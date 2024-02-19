import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subject } from '../../../../shared/models/Subject';
import { SubjectService } from '../../../../shared/services/SubjectService';
import { Result } from '../../../../shared/models/Result';
import { TeacherService } from '../../../../shared/services/TeacherService';


@Component({
  selector: 'dialog-assign-teacher',
  templateUrl: 'dialog-assign-teacher.component.html'
})
export class DialogAssignTeacher {
  subjects: Subject[] = [];
  teacherSubjects: Subject[] = [];
  constructor(
    public dialogRef: MatDialogRef<DialogAssignTeacher>,
    private _subjectService: SubjectService,
    private _teacherService: TeacherService,
    @Inject(MAT_DIALOG_DATA) public data: number) {
    this.getAllSubjects();
    this.getTeacherSubjects();
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

  getTeacherSubjects() {
    this._teacherService.getSubjects(this.data).subscribe({
      next: (result: Result<Subject[]>) => {
        this.teacherSubjects = result.data ?? [];
      },
      error: (err) => console.log(err),
    })
  }

  assign(subjectId: number) {
    this._teacherService.assign(this.data, subjectId).subscribe({
      next: (result: Result<boolean>) => {

      },
      error: (err) => console.log(err),
      complete: () => {
        this.getTeacherSubjects();
      }
    })
  }
}
