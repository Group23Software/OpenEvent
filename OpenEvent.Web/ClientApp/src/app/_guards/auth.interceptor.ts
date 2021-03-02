import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse
} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Observable, throwError} from "rxjs";
import {AuthService} from "../_Services/auth.service";
import {catchError, map} from "rxjs/operators";
import {TriggerService} from "../_Services/trigger.service";
import {MatSnackBar} from "@angular/material/snack-bar";

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor
{
  constructor (private authService: AuthService, private trigger: TriggerService, private snackBar: MatSnackBar)
  {
  }

  intercept (req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
  {
    this.trigger.loading.emit(true);

    let token = this.authService.GetToken();

    if (token != null)
    {
      console.log("using bearer token");
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(req).pipe(
      map((event: HttpEvent<any>) =>
      {
        console.log("there was an http event", event);
        if (event instanceof HttpResponse) this.trigger.loading.emit(false);
        return event;
      }),
      catchError((error: HttpErrorResponse) =>
      {
        console.log("there was an error response");
        this.trigger.loading.emit(false);
        this.snackBar.open(error.message, 'close', {
          duration: 1500,
          horizontalPosition: 'right',
          verticalPosition: 'top'
        });
        return throwError(error);
      }));
  }
}
