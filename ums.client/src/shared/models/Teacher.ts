export class Teacher {
  id: number;
  creatorUserId: string;
  name: string | null = null;
  creationDate?: Date;
  modificationUserId?: string;
  modificationDate?: Date;
  isDeleted: boolean;
  deletionUserId?: string;
  deletionDate?: Date;

  constructor(
    id: number,
    creatorUserId: string,
    name: string | null = null,
    isDeleted: boolean,
    creationDate?: Date,
    modificationUserId?: string,
    modificationDate?: Date,
    deletionUserId?: string,
    deletionDate?: Date
  ) {
    this.id = id;
    this.creatorUserId = creatorUserId;
    this.name = name;
    this.creationDate = creationDate;
    this.modificationUserId = modificationUserId;
    this.modificationDate = modificationDate;
    this.isDeleted = isDeleted;
    this.deletionUserId = deletionUserId;
    this.deletionDate = deletionDate;
  }
}
