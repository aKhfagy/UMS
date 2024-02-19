export class Subject {
  id: number;
  name: string | null;
  credit: number;
  teacherId?: number;
  creatorUserId: string;
  creationDate?: Date | null;
  modificationUserId?: string | null;
  modificationDate?: Date | null;
  isDeleted: boolean;
  deletionUserId?: string | null;
  deletionDate?: Date | null;

  constructor(
    id: number,
    name: string | null,
    credit: number,
    isDeleted: boolean,
    creatorUserId: string,
    teacherId?: number,
    creationDate?: Date | null,
    modificationUserId?: string | null,
    modificationDate?: Date | null,
    deletionUserId?: string | null,
    deletionDate?: Date | null
  ) {
    this.id = id;
    this.name = name;
    this.credit = credit;
    this.teacherId = teacherId;
    this.creatorUserId = creatorUserId;
    this.creationDate = creationDate;
    this.modificationUserId = modificationUserId;
    this.modificationDate = modificationDate;
    this.isDeleted = isDeleted;
    this.deletionUserId = deletionUserId;
    this.deletionDate = deletionDate;
  }
}
