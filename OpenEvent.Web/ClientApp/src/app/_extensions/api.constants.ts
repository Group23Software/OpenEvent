let authPaths: ApiAuthPaths = {
  BasePath: '/api/auth',
  Login: this.BasePath + '/login',
  Authenticate: this.BasePath + '/authenticateToken',
  UpdatePassword: this.BasePath + '/updatePassword',
}

let userPaths: ApiUserPaths =
{
  BasePath: '/api/user',
  Account: this.BasePath + '/account',
  EmailExists: this.BasePath + '/emailExists',
  PhoneExists: this.BasePath + '/phoneExists',
  UserNameExists: this.BasePath + '/userNameExists',
  UpdateAvatar: this.BasePath + '/updateAvatar',
  UpdateUserName: this.BasePath + '/update',
}

interface ApiAuthPaths
{
  readonly BasePath: string;
  readonly Login: string;
  readonly Authenticate: string;
  readonly UpdatePassword: string;
}

interface ApiUserPaths
{
  readonly BasePath: string;
  readonly Account: string;
  readonly UserNameExists: string;
  readonly EmailExists: string;
  readonly PhoneExists: string;
  readonly UpdateUserName: string;
  readonly UpdateAvatar: string;
}
