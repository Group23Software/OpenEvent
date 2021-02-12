import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';

import {AppComponent} from './app.component';
import {LoginComponent} from './login/login.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {CookieService} from "ngx-cookie-service";
import {ExploreComponent} from './explore/explore.component';
import {AuthGuard} from "./_guards/auth.guard";
import {AuthInterceptor} from "./_guards/auth.interceptor";
import {CreateAccountComponent} from './login/create-account/create-account.component';
import {ConfirmDialogComponent} from './_extensions/confirm-dialog/confirm-dialog.component';
import {PublicNavComponent} from "./navs/public-nav/public-nav.component";
import {SharedModule} from "./shared.module";
import {EventComponent} from "./event/event/event.component";
import {SearchComponent} from "./search/search.component";
import {EventPreviewComponent} from "./event/event-preview/event-preview.component";


@NgModule({
  declarations: [
    AppComponent,
    PublicNavComponent,
    LoginComponent,
    ExploreComponent,
    CreateAccountComponent,
    ConfirmDialogComponent,
    SearchComponent,
    EventPreviewComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    BrowserAnimationsModule,
    SharedModule,
    RouterModule.forRoot([
      {path: '', redirectTo: 'login', pathMatch: 'full'},
      // {
      //   path: 'main',
      //   loadChildren: () => import('./main.module').then(m => m.MainModule),
      //   pathMatch: 'full',
      //   canActivate: [AuthGuard]
      // },
      {
        path: 'search', component: SearchComponent, canActivate: [AuthGuard]
      },
      {path: 'login', component: LoginComponent},
      {
        path: 'user',
        loadChildren: () => import('./user.module').then(m => m.UserModule),
        canActivate: [AuthGuard]
      },
      {
        path: 'host',
        loadChildren: () => import('./host.module').then(m => m.HostModule),
        canActivate: [AuthGuard]
      },
      {
        path: 'event/:id',
        component: EventComponent,
        canActivate: [AuthGuard]
      },
    ]),
  ],
  providers: [CookieService, {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule
{
}
