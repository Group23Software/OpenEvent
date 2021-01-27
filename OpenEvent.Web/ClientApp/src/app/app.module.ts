import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';

import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {LoginComponent} from './login/login.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatInputModule} from "@angular/material/input";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatIconModule} from "@angular/material/icon";
import {MatButtonModule} from "@angular/material/button";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {LoadingComponent} from './loading/loading.component';
import {CookieService} from "ngx-cookie-service";
import {ExploreComponent} from './explore/explore.component';
import {AccountComponent} from './account/account.component';
import {AuthGuard} from "./_guards/auth.guard";
import {AuthInterceptor} from "./_guards/auth.interceptor";
import { CreateAccountComponent } from './login/create-account/create-account.component';
import {MatCheckboxModule} from "@angular/material/checkbox";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    LoginComponent,
    LoadingComponent,
    ExploreComponent,
    AccountComponent,
    CreateAccountComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      // {path: '', component: HomeComponent, pathMatch: 'full'},
      {path: 'login', component: LoginComponent},
      {path: 'account', component: AccountComponent, canActivate: [AuthGuard]}
    ]),
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatCheckboxModule,
  ],
  providers: [CookieService, {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule
{
}
