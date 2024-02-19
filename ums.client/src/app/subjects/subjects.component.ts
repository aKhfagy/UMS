import { Component, OnInit } from '@angular/core';
import { Subject } from '../../shared/models/Subject';
import { SubjectService } from '../../shared/services/SubjectService';
import { Result } from '../../shared/models/Result';
import { HttpStatusCode } from '@angular/common/http';
import { UserProxy } from '../../shared/services/UserService';
import { DialogManageSubject } from './modals/manage/dialog-manage-subject.component';
import { ManageObj } from '../../shared/models/ManageObj';
import { MatDialog } from '@angular/material/dialog';
import { makeNewSubject } from '../../shared/Utils';
import { PageEvent } from '@angular/material/paginator';
import { IsPagedOutputObj } from '../../shared/models/IsPagedOutputObj';

@Component({
  selector: 'app-subjects',
  templateUrl: './subjects.component.html',
})
export class SubjectsComponent implements OnInit {
  public subjects: Subject[];
  public userId: number | undefined;
  public showDeleted: boolean;
  public subject: Subject;
  public page: number;
  public totalRecords: number;

  displayedColumns: string[] = [
    'name',
    'actions',
  ];
  constructor(
    private _subjectService: SubjectService,
    private _userProxy: UserProxy,
    private dialog: MatDialog) {
    this.subjects = [];
    this.showDeleted = false;
    this.page = 1;
    this.totalRecords = 0;
    this.subject = makeNewSubject();
  }
  ngOnInit(): void {
    this.getSubjects();
  }

  getSubjects() {
    this._subjectService.isPaged(this.page, this.showDeleted).subscribe({
      next: (result: Result<IsPagedOutputObj<Subject>>) => {
        if (result.statusCode == HttpStatusCode.Ok) {
          this.subjects = result.data?.values ?? []
          this.totalRecords = result.data?.totalCount ?? 0;
        }
      },
      error: (error) => {
        console.log(error);
      }
    });
  }

  delete(id: number) {
    this._subjectService.delete(id).subscribe({
      next: (result: Result<boolean>) => {
        if (result.data != null && result.statusCode == HttpStatusCode.Ok) {

        }
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        this.getSubjects();
      }
    })
  }

  restore(id: number) {
    this._subjectService.restore(id).subscribe({
      next: (result: Result<boolean>) => {
        if (result.data != null && result.statusCode == HttpStatusCode.Ok) {

          this.getSubjects();
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
    this.getSubjects();
  }

  openManage(subject: Subject) {
    const ref = this.dialog.open(DialogManageSubject, {
      data: new ManageObj<Subject>(subject)
    });

    ref.afterClosed().subscribe(result => {
      this.getSubjects();
      this.subject.name = '';
    })
  
  }

  changePage(event: PageEvent) {
    this.page = event.pageIndex + 1;
    this.getSubjects();
  }
}
