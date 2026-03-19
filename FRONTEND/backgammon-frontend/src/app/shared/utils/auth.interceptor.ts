import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { tap } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const toastr = inject(ToastrService);

  const token = authService.getToken();

  if (token) {
    const clonedReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`),
    });

    return next(clonedReq).pipe(
      tap({
        error: (err: any) => {
          if (err.status == 401) {
            authService.deleteToken();
            router.navigateByUrl('/signin');
            setTimeout(() => {
              toastr.info('Please login again.', 'Session Expired!');
            }, 1500);
          } else if (err.status == 403) {
            toastr.error(
              "Oops! It seems like you're not authorized to perform the action.",
            );
          }
        },
      }),
    );
  } else {
    return next(req);
  }
};
