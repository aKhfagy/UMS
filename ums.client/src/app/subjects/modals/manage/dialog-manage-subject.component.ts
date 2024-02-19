import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ManageObj } from '../../../../shared/models/ManageObj';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Result } from '../../../../shared/models/Result';
import { SubjectService } from '../../../../shared/services/SubjectService';
import { Subject } from '../../../../shared/models/Subject';


@Component({
  selector: 'dialog-manage-subject',
  templateUrl: 'dialog-manage-subject.component.html'
})
export class DialogManageSubject {
  public form: FormGroup;
  constructor(
    public dialogRef: MatDialogRef<DialogManageSubject>,
    private _subjectService: SubjectService,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: ManageObj<Subject>) {
    this.form = this.fb.group({
      name: [data.obj.name, Validators.required]
    });
  }

  close() {
    this.dialogRef.close();
  }
  save() {
    let subject = this.data.obj;
    subject.name = this.name?.value;
    this.manage(subject);
  }
  manage(subject: Subject) {

    if (subject.id > 0) {
      this._subjectService.edit(subject).subscribe({
        next: (result: Result<Subject>) => {

        },
        error: (err) => {
          console.log(err);
        },
        complete: () => {
          this.close();
        }
      })
    }
    else {
      this._subjectService.add(subject).subscribe({
        next: (result: Result<Subject>) => {

        },
        error: (err) => {
          console.log(err);
        },
        complete: () => {
          this.close();
        }
      })
    }

  }

  get name() {
    return this.form.get('name');
  }
}
