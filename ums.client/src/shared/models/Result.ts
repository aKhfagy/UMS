import { HttpStatusCode } from "@angular/common/http";

export class Result<T> {
  data: T | null;
  message: string;
  statusCode: HttpStatusCode;

  constructor(data: T | null = null, message: string = '', statusCode: HttpStatusCode) {
    this.data = data;
    this.message = message;
    this.statusCode = statusCode;
  }
}
