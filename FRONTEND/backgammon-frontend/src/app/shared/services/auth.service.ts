import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { TOKEN_KEY } from '../utils/constants/constants';
import { AuthResponse } from '../models/auth-response.models';
import { Observable } from 'rxjs';
import { RegisterRequest } from '../../features/auth/registration/models/register-request.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {}

  createUser(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      environment.apiBaseUrl + '/users/register',
      request,
    );
  }

  signIn(formData: any) {
    return this.http.post<AuthResponse>(
      environment.apiBaseUrl + '/auth/login',
      formData,
    );
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;

    try {
      let base64 = token.split('.')[1];
      if (!base64) return false;
      base64 = base64.replace(/-/g, '+').replace(/_/g, '/');
      while (base64.length % 4) base64 += '=';
      const payload = JSON.parse(atob(base64));
      return Date.now() < payload.exp * 1000;
    } catch (err) {
      console.error('Invalid token format', err);
      return false;
    }
  }

  saveToken(token: string) {
    localStorage.setItem(TOKEN_KEY, token);
  }

  getToken() {
    return localStorage.getItem(TOKEN_KEY);
  }

  deleteToken() {
    localStorage.removeItem(TOKEN_KEY);
  }

  getClaims() {
    const token = this.getToken();
    if (!token) return null;

    try {
      let base64 = token.split('.')[1];
      base64 = base64.replace(/-/g, '+').replace(/_/g, '/');
      while (base64.length % 4) base64 += '=';

      return JSON.parse(atob(base64));
    } catch (err) {
      console.error('Invalid token format', err);
      return null;
    }
  }
}
