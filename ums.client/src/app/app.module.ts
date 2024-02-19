import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientJsonpModule } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { StudentsComponent } from './students/students.component';
import { DialogEnrollStudent } from './students/modals/enroll/dialog-enroll-student.component';
import { DialogManageStudent } from './students/modals/manage/dialog-manage-student.component';
import { DialogAssignTeacher } from './teachers/modals/assign/dialog-assign-teacher.component';
import { SubjectsComponent } from './subjects/subjects.component';
import { TeachersComponent } from './teachers/teachers.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIcon } from '@angular/material/icon';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule, MatButton } from '@angular/material/button';
import {
  MatDialogModule,
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose, } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatListModule } from '@angular/material/list';
import { ReactiveFormsModule } from '@angular/forms';
import { DialogManageSubject } from './subjects/modals/manage/dialog-manage-subject.component';
import { DialogManageTeacher } from './teachers/modals/manage/dialog-manage-teacher.component';
import { MatPaginatorModule } from '@angular/material/paginator';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MatTabsModule,
    HttpClientModule,
    HttpClientJsonpModule,
    AppRoutingModule,
    MatToolbarModule,
    MatIcon,
    MatIconModule,
    MatButton,
    MatButtonModule,
    MatTableModule,
    MatDialogModule,
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatDialogClose,
    MatInputModule,
    MatSelectModule,
    MatFormFieldModule,
    MatListModule,
    ReactiveFormsModule,
    MatPaginatorModule
  ],
  declarations: [
    AppComponent,
    HomeComponent,
    StudentsComponent,
    SubjectsComponent,
    TeachersComponent,
    DialogEnrollStudent,
    DialogManageStudent,
    DialogAssignTeacher,
    DialogManageSubject,
    DialogManageTeacher,
  ]
})
export class AppModule {

}
