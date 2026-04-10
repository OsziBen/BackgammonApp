import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../shared/services/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { AppRoutes } from '../../../../app.routes';
import { RegisterRequest } from '../models/register-request.model';

@Component({
  selector: 'app-registration',
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './registration.component.html',
})
export class RegistrationComponent implements OnInit {
  @Output() close = new EventEmitter<void>();

  @Output() switchToLogin = new EventEmitter<void>();

  form: FormGroup;
  isSubmitted = false;

  constructor(
    public formBuilder: FormBuilder,
    private service: AuthService,
    private toastr: ToastrService,
    private router: Router,
  ) {
    this.form = this.formBuilder.group(
      {
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        userName: ['', Validators.required],
        emailAddress: ['', [Validators.required, Validators.email]],
        dateOfBirth: ['', Validators.required],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(8),
            Validators.pattern(/(?=.*[^a-zA-Z0-9 ])/),
          ],
        ],
        confirmPassword: ['', Validators.required],
      },
      { validators: this.passwordMatchValidator },
    );
  }

  onSwitchToLogin(): void {
    this.switchToLogin.emit();
  }

  ngOnInit(): void {
    if (this.service.isLoggedIn()) {
      this.router.navigateByUrl('/dashboard');
    }
  }

  passwordMatchValidator: ValidatorFn = (control: AbstractControl) => {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (!password || !confirmPassword) {
      return null;
    }

    if (password.value != confirmPassword.value)
      confirmPassword?.setErrors({ passwordMismatch: true });
    else {
      if (confirmPassword.hasError('passwordMismatch')) {
        confirmPassword.updateValueAndValidity({
          onlySelf: true,
          emitEvent: false,
        });
      }
    }

    return null;
  };

  get passwordErrorKey(): string | null {
    const errors = this.form.get('password')?.errors;

    if (!errors) {
      return null;
    }

    return Object.keys(errors)[0];
  }

  onSubmit(): void {
    this.isSubmitted = true;

    if (!this.form.valid) return;

    const request: RegisterRequest = {
      firstName: this.form.value.firstName,
      lastName: this.form.value.lastName,
      userName: this.form.value.userName,
      emailAddress: this.form.value.emailAddress,
      password: this.form.value.password,
      dateOfBirth: this.form.value.dateOfBirth,
    };

    this.service.createUser(request).subscribe({
      next: (res) => {
        this.toastr.success('Registration successful');
        this.service.saveToken(res.jwtToken);
        this.close.emit();
        this.router.navigate(['/', AppRoutes.home]);
      },
      error: (err: HttpErrorResponse) => {
        if (err.status === 400) {
          this.toastr.error('Registration failed');
        } else {
          console.error(err);
          this.toastr.error('Unexpected error occurred');
        }
      },
    });
  }

  hasDisplayableError(controlName: string): boolean {
    const control = this.form.get(controlName);

    return !!(
      control?.invalid &&
      (this.isSubmitted || control.touched || control.dirty)
    );
  }
}
