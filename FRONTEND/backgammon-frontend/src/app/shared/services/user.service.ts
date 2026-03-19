import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(
    private http: HttpClient,
    private authService: AuthService,
  ) {}

  getUserProfile() {
    return this.http.get(environment.apiBaseUrl + '/AppUser/UserProfile'); // TODO!
  }
}
