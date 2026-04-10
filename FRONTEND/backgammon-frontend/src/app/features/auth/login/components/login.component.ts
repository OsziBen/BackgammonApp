import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../../shared/services/auth.service';
import { AppRoutes } from '../../../../app.routes';
import { LoginRequest } from '../models/login-request.model';
import { AuthResponse } from '../../../../shared/models/auth-response.models';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styles: ``,
})
export class LoginComponent implements OnInit {
  @Output() close = new EventEmitter<void>();

  @Output() switchToRegister = new EventEmitter<void>();

  form: FormGroup;
  isSubmitted: boolean = false;

  constructor(
    public formBuilder: FormBuilder,
    private service: AuthService,
    private router: Router,
    private toastr: ToastrService,
  ) {
    this.form = this.formBuilder.group({
      emailAddress: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onSwitchToRegister(): void {
    this.switchToRegister.emit();
  }

  ngOnInit(): void {
    if (this.service.isLoggedIn()) {
      this.router.navigate(['/', AppRoutes.home]);
    }
  }

  onSubmit() {
    this.isSubmitted = true;
    if (!this.form.valid) return;

    const request: LoginRequest = {
      EmailAddress: this.form.value.emailAddress,
      Password: this.form.value.password,
    };

    this.service.signIn(request).subscribe({
      next: (res: AuthResponse) => {
        this.toastr.success('Login successful');
        this.service.saveToken(res.jwtToken);
        this.close.emit();
        this.router.navigate(['/', AppRoutes.home]);
      },
      error: (err) => {
        if (err.status === 400) {
          this.toastr.error('Incorrect email or password.', 'Login failed');
        } else {
          console.error('Login error:', err);
        }
      },
    });
  }

  hasDisplayableError(controlName: string): Boolean {
    const control = this.form.get(controlName);

    return !!(
      control?.invalid &&
      (this.isSubmitted || control.touched || control.dirty)
    );
  }
}
