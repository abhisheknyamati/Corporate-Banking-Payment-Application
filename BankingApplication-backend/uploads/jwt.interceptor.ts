import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../service/auth.service';
import { switchMap, take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    
    return this.authService.userToken.pipe(
      take(1),
      switchMap((token: string | null) => {
        const isApiUrl = request.url.startsWith('http://localhost:8080/studentapp/');
        
        
        if (token && isApiUrl) {
          request = request.clone({
            setHeaders: {
              Authorization: `Bearer ${token}`
            }
          });
        }

        // Pass the request to the next handler
        return next.handle(request);
      })
    );
  }
}
