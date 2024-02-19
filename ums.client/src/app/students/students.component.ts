import { Component, OnInit } from '@angular/core';
import { StudentService } from '../../shared/services/StudentService';
import { Student } from '../../shared/models/Student';
import { MatDialog } from '@angular/material/dialog';
import { Result } from '../../shared/models/Result';
import { DialogEnrollStudent } from './modals/enroll/dialog-enroll-student.component';
import { DialogManageStudent } from './modals/manage/dialog-manage-student.component';
import { HttpStatusCode } from '@angular/common/http';
import { UserProxy } from '../../shared/services/UserService';
import { ManageObj } from '../../shared/models/ManageObj';
import { makeNewStudent } from '../../shared/Utils';
import { IsPagedOutputObj } from '../../shared/models/IsPagedOutputObj';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-students',
  templateUrl: './students.component.html',
})
export class StudentsComponent implements OnInit {
  public students: Student[];
  public showDeleted: boolean;
  public page: number;
  public totalRecords: number;
  public student: Student;

  displayedColumns: string[] = [
    'name',
    'actions'
  ];
  constructor(
    private _studentService: StudentService,
    private _userProxy: UserProxy,
    private dialog: MatDialog) {
    this.students = [];
    this.showDeleted = false;
    this.page = 1;
    this.totalRecords = 0;
    this.student = makeNewStudent();
  }
  ngOnInit(): void {
    this.getStudents();
  }

  getStudents() {
    this._studentService.isPaged(this.page, this.showDeleted).subscribe({
      next: (result: Result<IsPagedOutputObj<Student>>) => {
        if (result.statusCode == HttpStatusCode.Ok) {
          this.students = result.data?.values ?? [];
          this.totalRecords = result.data?.totalCount ?? 0;
        }
      },
      error: (error) => {
        console.log(error);
      }
    });
  }

  openEnroll(studentId: number) {
    this.dialog.open(DialogEnrollStudent, {
      data: studentId,
    });
  }

  openManage(student: Student) {
    const ref = this.dialog.open(DialogManageStudent, {
      data: new ManageObj<Student>(student),
    });
    ref.afterClosed().subscribe(result => {
      this.getStudents();
      this.student.name = '';
    });

  }

  delete(id: number) {
    this._studentService.delete(id).subscribe({
      next: (result: Result<boolean>) => {
        if (result.data != null && result.statusCode == HttpStatusCode.Ok) {
          this.getStudents();
        }
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
      }
    })
  }

  restore(id: number) {
    this._studentService.restore(id).subscribe({
      next: (result: Result<boolean>) => {
        if (result.data != null && result.statusCode == HttpStatusCode.Ok) {

          this.getStudents();
        }
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
      }
    })
  }

  filter() {
    this.showDeleted = !this.showDeleted;
    this.getStudents();
  }

  changePage(event: PageEvent) {
    this.page = event.pageIndex + 1;
    this.getStudents();
  }
}
