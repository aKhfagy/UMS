import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subject } from '../../../../shared/models/Subject';
import { SubjectService } from '../../../../shared/services/SubjectService';
import { StudentService } from '../../../../shared/services/StudentService';
import { UserProxy } from '../../../../shared/services/UserService';
import { Result } from '../../../../shared/models/Result';
import { StudentSubject } from '../../../../shared/models/StudentSubject';
import { HttpStatusCode } from '@angular/common/http';


@Component({
  selector: 'dialog-enroll-student',
  templateUrl: 'dialog-enroll-student.component.html'
})
export class DialogEnrollStudent {
  subjects: Subject[] = [];
  studentSubjects: StudentSubject[] = [];
  constructor(
    public dialogRef: MatDialogRef<DialogEnrollStudent>,
    private _subjectService: SubjectService,
    private _studentService: StudentService,
    @Inject(MAT_DIALOG_DATA) public data: number) {
    this.getAllSubjects();
    this.getStudentSubjects();
  }

  close() {
    this.dialogRef.close();
  }

  getAllSubjects() {
    this._subjectService.getAll().subscribe({
      next: (result: Result<Subject[]>) => {
        this.subjects = result.data ?? [];
      },
      error: (err) => console.log(err),
    })
  }

  getStudentSubjects() {
    this._studentService.getSubjects(this.data).subscribe({
      next: (result: Result<StudentSubject[]>) => {
        this.studentSubjects = result.data ?? [];
      },
      error: (err) => console.log(err),
    })
  }

  enroll(subjectId: number) {
    this._studentService.enroll(this.data, subjectId).subscribe({
      next: (result: Result<boolean>) => {
        if (result.data) {
          console.log(`Enrolled in subject with id: ${subjectId}`);
        }
        else {
          console.log(`Failed to enroll student in subject with id: ${subjectId}`);
        }
      },
      error: (err) => console.log(err),
      complete: () => {
        this.getStudentSubjects();
      }
    })
  }
}
