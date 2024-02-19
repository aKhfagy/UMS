import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ManageObj } from '../../../../shared/models/ManageObj';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Result } from '../../../../shared/models/Result';
import { Teacher } from '../../../../shared/models/Teacher';
import { TeacherService } from '../../../../shared/services/TeacherService';


@Component({
  selector: 'dialog-manage-teacher',
  templateUrl: 'dialog-manage-teacher.component.html'
})
export class DialogManageTeacher {
  public form: FormGroup;
  constructor(
    public dialogRef: MatDialogRef<DialogManageTeacher>,
    private _teacherService: TeacherService,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: ManageObj<Teacher>) {
    this.form = this.fb.group({
      name: [data.obj.name, Validators.required]
    });
  }

  close() {
    this.dialogRef.close();
  }
  save() {
    let teacher = this.data.obj;
    teacher.name = this.name?.value;
    this.manage(teacher);
  }
  manage(teacher: Teacher) {

    if (teacher.id > 0) {
      this._teacherService.edit(teacher).subscribe({
        next: (result: Result<Teacher>) => {

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
      this._teacherService.add(teacher).subscribe({
        next: (result: Result<Teacher>) => {

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
