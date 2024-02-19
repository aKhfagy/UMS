import { Subject } from "./Subject";

export class StudentSubject {
  id: number;
  studentId: number;
  subjectId: number;
  userId: number;
  grade: number | null | undefined;
  subject: Subject;
  isDeleted: boolean;

  constructor(id: number, studentId: number, subjectId: number, userId: number, grade: number | null | undefined, subject: Subject, isDeleted: boolean) {
    this.id = id;
    this.studentId = studentId;
    this.subjectId = subjectId;
    this.userId = userId;
    this.grade = grade;
    this.subject = subject;
    this.isDeleted = isDeleted;
  }
}
