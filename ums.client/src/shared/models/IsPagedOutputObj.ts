export class IsPagedOutputObj<T> {
  pageIndex: number;
  pageSize: number = 5;
  pageCount: number;
  totalCount: number;
  isDeleted: boolean;
  values: T[];

  constructor(pageIndex: number, pageSize: number, pageCount: number, totalCount: number, isDeleted: boolean, values: T[]) {
    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.pageCount = pageCount;
    this.totalCount = totalCount;
    this.isDeleted = isDeleted;
    this.values = values;
  }
}
