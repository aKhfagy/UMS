import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { StudentService } from '../../../../shared/services/StudentService';
import { ManageObj } from '../../../../shared/models/ManageObj';
import { Student } from '../../../../shared/models/Student';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Result } from '../../../../shared/models/Result';


@Component({
  selector: 'dialog-manage-student',
  templateUrl: 'dialog-manage-student.component.html'
})
export class DialogManageStudent {
  public form: FormGroup;
  constructor(
    public dialogRef: MatDialogRef<DialogManageStudent>,
    private _studentService: StudentService,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: ManageObj<Student>) {
    console.log(data);
    this.form = this.fb.group({
      name: [data.obj.name, Validators.required]
    });
  }

  close() {
    this.dialogRef.close();
  }
  save() {
    let student = this.data.obj;
    student.name = this.name?.value;
    this.manage(student);
  }
  manage(student: Student) {

    if (student.id > 0) {
      this._studentService.edit(student).subscribe({
        next: (result: Result<Student>) => {

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
      this._studentService.add(student).subscribe({
        next: (result: Result<Student>) => {

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
