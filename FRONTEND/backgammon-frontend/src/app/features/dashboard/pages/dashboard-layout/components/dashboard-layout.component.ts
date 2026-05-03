import { Component } from '@angular/core';
import { AuthService } from '../../../../../shared/services/auth.service';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AppRoutes } from '../../../../../app.routes';
import { LoginComponent } from '../../../../auth/login/components/login.component';
import { RegistrationComponent } from '../../../../auth/registration/components/registration.component';

@Component({
  selector: 'app-dashboard-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    RouterModule,
    LoginComponent,
    RegistrationComponent,
  ],
  templateUrl: './dashboard-layout.component.html',
  styleUrls: ['./dashboard-layout.component.css'],
})
export class DashboardLayoutComponent {
  AppRoutes = AppRoutes;
  showLogin = false;
  showRegister = false;

  readonly currentYear = new Date().getUTCFullYear();

  constructor(
    public authService: AuthService,
    private router: Router,
  ) {}

  openLogin() {
    this.showLogin = true;
    this.showRegister = false;
  }

  openRegister() {
    this.showRegister = true;
    this.showLogin = false;
  }

  closeModal() {
    this.showLogin = false;
    this.showRegister = false;
  }

  logout() {
    this.authService.deleteToken();
    this.router.navigate(['/']);
  }

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }
}
