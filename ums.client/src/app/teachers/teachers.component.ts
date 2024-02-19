import { Component, OnInit } from '@angular/core';
import { Teacher } from '../../shared/models/Teacher';
import { TeacherService } from '../../shared/services/TeacherService';
import { Result } from '../../shared/models/Result';
import { DialogAssignTeacher } from './modals/assign/dialog-assign-teacher.component';
import { MatDialog } from '@angular/material/dialog';
import { HttpStatusCode } from '@angular/common/http';
import { UserProxy } from '../../shared/services/UserService';
import { DialogManageTeacher } from './modals/manage/dialog-manage-teacher.component';
import { makeNewTeacher } from '../../shared/Utils';
import { ManageObj } from '../../shared/models/ManageObj';
import { PageEvent } from '@angular/material/paginator';
import { IsPagedOutputObj } from '../../shared/models/IsPagedOutputObj';

@Component({
  selector: 'app-teachers',
  templateUrl: './teachers.component.html',
})
export class TeachersComponent implements OnInit {
  public teachers: Teacher[];
  public userId: number | undefined;
  public showDeleted: boolean;
  public teacher: Teacher;
  public page: number;
  public totalRecords: number;

  displayedColumns: string[] = [
    'name',
    'actions',
  ];
  constructor(
    private _teacherService: TeacherService,
    private _userProxy: UserProxy,
    private dialog: MatDialog) {
    this.teachers = [];
    this.showDeleted = false;
    this.page = 1;
    this.totalRecords = 0;
    this.teacher = makeNewTeacher()
  }

  ngOnInit(): void {
    this.getTeachers();
  }

  getTeachers() {
    this._teacherService.isPaged(this.page, this.showDeleted).subscribe({
      next: (result: Result<IsPagedOutputObj<Teacher>>) => {
        if (result.statusCode == HttpStatusCode.Ok) {
          this.teachers = result.data?.values ?? []
          this.totalRecords = result.data?.totalCount ?? 0;
        }
      },
      error: (error) => {
        console.log(error);
      }
    });
  }

  delete(id: number) {
    this._teacherService.delete(id).subscribe({
      next: (result: Result<boolean>) => {
        if (result.data != null && result.statusCode == HttpStatusCode.Ok) {

        }
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        this.getTeachers();
      }
    })
  }
  restore(id: number) {
    this._teacherService.restore(id).subscribe({
      next: (result: Result<boolean>) => {
        if (result.data != null && result.statusCode == HttpStatusCode.Ok) {

          this.getTeachers();
        }
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
      }
    })
  }
  openDialog(teacherId: number) {
    this.dialog.open(DialogAssignTeacher, {
      data: teacherId,
    });
  }
  filter() {
    this.showDeleted = !this.showDeleted;
    this.getTeachers();
  }

  openManage(teacher: Teacher) {
    const ref = this.dialog.open(DialogManageTeacher, {
      data: new ManageObj<Teacher>(teacher)
    });

    ref.afterClosed().subscribe(result => {
      this.getTeachers();
      this.teacher.name = '';
    })

  }


  changePage(event: PageEvent) {
    this.page = event.pageIndex + 1;
    this.getTeachers();
  }
}
