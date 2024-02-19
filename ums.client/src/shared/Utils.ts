import { Student } from "./models/Student";
import { Subject } from "./models/Subject";
import { Teacher } from "./models/Teacher";

export function makeNewStudent(): Student {
  let student = new Student(
    0,
    '',
    false,
    ''
  );

  return student;
}
export function makeNewSubject(): Subject {
  let subject = new Subject(
    0,
    '',
    0,
    false,
    ''
  );

  return subject;
}

export function makeNewTeacher(): Teacher {
  let teacher = new Teacher(
    0,
    '',
    '',
    false,
  );

  return teacher;
}
