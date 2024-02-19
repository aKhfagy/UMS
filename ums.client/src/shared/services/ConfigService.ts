import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  private apiUrl = 'https://localhost:7289';

  constructor() { }

  getApiUrl(): string {
    return this.apiUrl;
  }
}
