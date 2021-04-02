import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {HTTP_INTERCEPTORS} from '@angular/common/http';
import {RouteReuseStrategy, RouterModule} from '@angular/router';

import {AppComponent} from './app.component';
import {LoginComponent} from './login/login.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {CookieService} from "ngx-cookie-service";
import {ExploreComponent} from './explore/explore.component';
import {AuthGuard} from "./_guards/auth.guard";
import {AuthInterceptor} from "./_guards/auth.interceptor";
import {CreateAccountComponent} from './login/create-account/create-account.component';
import {ConfirmDialogComponent} from './_extensions/confirm-dialog/confirm-dialog.component';
import {SharedModule} from "./shared.module";
import {EventComponent} from "./event/event/event.component";
import {SearchComponent} from "./search/search.component";
import {NgxStripeModule} from "ngx-stripe";
import {LandingComponent} from './landing/landing.component';
import {PublicNavComponent} from "./navs/public-nav/public-nav.component";
import {UserNavComponent} from "./navs/user-nav/user-nav.component";
import {ForgotPasswordComponent} from "./login/forgot-password/forgot-password.component";
import {PageNotFoundComponent} from "./page-not-found/page-not-found.component";
import {ForgotPasswordDialogComponent} from "./login/forgot-password-dialog/forgot-password-dialog.component";
import {RouteReuse} from "./_extensions/RouteReuse";
import {MockAddressFormComponent} from "./address-form/address-form.mock";
import {MockImageListComponent} from "./image-list/image-list.mock";
import {MockCategoryListComponent} from "./category-list/category-list.mock";
import {MockMarkdown} from "./_extensions/markdown.mock";
import {MockSocialComponent} from "./event/social/social.mock";
import {MockSocialLinksFormComponent} from "./event/create-event/social-links-form/social-links.mock";
import {MockThumbnailEditComponent} from "./thumbnail-edit/thumbnail-edit.mock";

@NgModule({
  declarations: [
    MockAddressFormComponent,
    MockImageListComponent,
    MockCategoryListComponent,
    MockMarkdown,
    MockSocialComponent,
    MockSocialLinksFormComponent,
    MockThumbnailEditComponent
  ]
})
export class MockModule
{

}


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ExploreComponent,
    CreateAccountComponent,
    ConfirmDialogComponent,
    SearchComponent,
    LandingComponent,
    PublicNavComponent,
    UserNavComponent,
    ForgotPasswordComponent,
    ForgotPasswordDialogComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    BrowserAnimationsModule,
    SharedModule,
    NgxStripeModule.forRoot('pk_test_51ILW9dK2ugLXrgQXdjbh4TSUPcp3TxQeB51iiecfFZMv7ZRW2durBaN1H8KFfIIyQEEmGfeYRaRvr6KS170oOuOA00Ledf7YEh'),
    RouterModule.forRoot([
      {path: '', pathMatch: 'full', component: LandingComponent},
      {path: 'explore', component: ExploreComponent, canActivate: [AuthGuard]},
      {
        path: 'search', component: SearchComponent,
        data: {
          reuse: true
        }
      },
      {path: 'login', component: LoginComponent},
      {
        path: 'user',
        loadChildren: () => import('./user.module').then(m => m.UserModule),
        canActivate: [AuthGuard]
      },
      {
        path: 'host',
        loadChildren: () => import('./host/host.module').then(m => m.HostModule),
        canActivate: [AuthGuard]
      },
      {
        path: 'event/:id',
        component: EventComponent
      },
      {
        path: 'forgot/:id',
        component: ForgotPasswordComponent
      },
      {path: '**', component: PageNotFoundComponent}
    ]),
    MockModule
  ],
  providers: [
    CookieService,
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    // {provide: RouteReuseStrategy, useClass: RouteReuse}
  ],
  bootstrap: [AppComponent],
})
export class AppModule
{
}


