export class Student {
  id: number;
  name: string | null;
  creatorUserId: string;
  creationDate?: Date;
  modificationUserId?: string | null;
  modificationDate?: Date | null;
  isDeleted: boolean;
  deletionUserId?: string | null;
  deletionDate?: Date | null;

  constructor(
    id: number,
    creatorUserId: string,
    isDeleted: boolean,
    name: string | null,
    creationDate?: Date,
    modificationUserId?: string | null,
    modificationDate?: Date | null,
    deletionUserId?: string | null,
    deletionDate?: Date | null
  ) {
    this.id = id;
    this.name = name;
    this.creatorUserId = creatorUserId;
    this.creationDate = creationDate;
    this.modificationUserId = modificationUserId;
    this.modificationDate = modificationDate;
    this.isDeleted = isDeleted;
    this.deletionUserId = deletionUserId;
    this.deletionDate = deletionDate;
  }
}
