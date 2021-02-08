let authPaths: ApiAuthPaths = {
  BasePath: 'api/auth',
  Authenticate: '',
  UpdatePassword: '',
  Login: ''
}

authPaths = {
  ...authPaths,
  Login: authPaths.BasePath + '/login',
  Authenticate: authPaths.BasePath + '/authenticateToken',
  UpdatePassword: authPaths.BasePath + '/updatePassword',
}

let userPaths: ApiUserPaths =
  {
    BasePath: 'api/user',
    Account: null,
    EmailExists: null,
    PhoneExists: null,
    UserNameExists: null,
    UpdateAvatar: null,
    UpdateUserName: null,
    UpdateThemePreference: null,
  }

userPaths = {
  ...userPaths,
  Account: userPaths.BasePath + '/account',
  EmailExists: userPaths.BasePath + '/emailExists',
  PhoneExists: userPaths.BasePath + '/phoneExists',
  UserNameExists: userPaths.BasePath + '/userNameExists',
  UpdateAvatar: userPaths.BasePath + '/updateAvatar',
  UpdateUserName: userPaths.BasePath + '/updateUserName',
  UpdateThemePreference: userPaths.BasePath + '/updateThemePreference'
}

let eventPaths: ApiEventPaths = {
  BasePath: 'api/event',
  CancelEvent: '',
  GetAllHostsEvents: '',
  GetForPublic: '',
  GetAllCategories: '',
  GetForHost: '',
  Update: '',
  Cancel: ''
}

eventPaths = {
  ...eventPaths,
  CancelEvent: eventPaths.BasePath + '/cancel',
  GetForPublic: eventPaths.BasePath + '/public',
  GetAllHostsEvents: eventPaths.BasePath + '/host',
  GetAllCategories: eventPaths.BasePath + '/categories',
  GetForHost: eventPaths.BasePath + '/forHost',
  Update: eventPaths.BasePath + '/update',
  Cancel: eventPaths.BasePath + '/cancel'
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
  readonly UpdateThemePreference: string;
}

interface ApiEventPaths
{
  readonly BasePath: string;
  readonly CancelEvent: string;
  readonly GetForPublic: string;
  readonly GetAllHostsEvents: string;
  readonly GetAllCategories: string;
  readonly GetForHost: string;
  readonly Update: string;
  readonly Cancel: string;
}

export const UserPaths: ApiUserPaths = userPaths;
export const AuthPaths: ApiAuthPaths = authPaths;
export const EventPaths: ApiEventPaths = eventPaths;
