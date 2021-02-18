import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {AuthService} from "../_Services/auth.service";
import {map} from "rxjs/operators";
import {TriggerService} from "../_Services/trigger.service";

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor
{
  constructor (private authService: AuthService, private trigger: TriggerService)
  {
  }

  intercept (req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
  {
    this.trigger.loading.emit(true);

    let token = this.authService.GetToken();

    if (token != null) {
      console.log("using bearer token");
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }
    return next.handle(req).pipe(map((event:HttpEvent<any>) => {
      if (event instanceof HttpResponse) this.trigger.loading.emit(false);
      return event;
    }));
  }
}
