let authPaths: ApiAuthPaths = {
  BasePath: 'api/auth',
  Authenticate: '',
  UpdatePassword: '',
  Login: '',
  ForgotPassword: ''
}

authPaths = {
  ...authPaths,
  Login: authPaths.BasePath + '/login',
  Authenticate: authPaths.BasePath + '/authenticateToken',
  UpdatePassword: authPaths.BasePath + '/updatePassword',
  ForgotPassword: authPaths.BasePath + '/forgot'
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
    UpdateAddress: null,
    GetUsersAnalytics: null
  }

userPaths = {
  ...userPaths,
  Account: userPaths.BasePath + '/account',
  EmailExists: userPaths.BasePath + '/emailExists',
  PhoneExists: userPaths.BasePath + '/phoneExists',
  UserNameExists: userPaths.BasePath + '/userNameExists',
  UpdateAvatar: userPaths.BasePath + '/updateAvatar',
  UpdateUserName: userPaths.BasePath + '/updateUserName',
  UpdateThemePreference: userPaths.BasePath + '/updateThemePreference',
  UpdateAddress: userPaths.BasePath + '/updateAddress',
  GetUsersAnalytics: userPaths.BasePath + '/analytics'
}

let eventPaths: ApiEventPaths = {
  BasePath: 'api/event',
  CancelEvent: '',
  GetAllHostsEvents: '',
  GetForPublic: '',
  GetAllCategories: '',
  GetForHost: '',
  Update: '',
  Cancel: '',
  Search: '',
  Explore: '',
  Analytics: '',
  DownVote: ''
}

eventPaths = {
  ...eventPaths,
  CancelEvent: eventPaths.BasePath + '/cancel',
  GetForPublic: eventPaths.BasePath + '/public',
  GetAllHostsEvents: eventPaths.BasePath + '/host',
  GetAllCategories: eventPaths.BasePath + '/categories',
  GetForHost: eventPaths.BasePath + '/forHost',
  Update: eventPaths.BasePath + '/update',
  Cancel: eventPaths.BasePath + '/cancel',
  Search: eventPaths.BasePath + '/search',
  Explore: eventPaths.BasePath + '/explore',
  Analytics: eventPaths.BasePath + '/analytics',
  DownVote: eventPaths.BasePath + '/downvote'
}

let paymentPaths: ApiPaymentPaths = {
  BasePath: 'api/payment',
  AddPaymentMethod: '',
  MakePaymentDefault: '',
  RemovePaymentMethod: ''
}

paymentPaths = {
  ...paymentPaths,
  AddPaymentMethod: paymentPaths.BasePath + '/AddPaymentMethod',
  RemovePaymentMethod: paymentPaths.BasePath + '/RemovePaymentMethod',
  MakePaymentDefault: paymentPaths.BasePath + '/MakePaymentDefault'
}

let bankingPaths: ApiBankingPaths = {
  BasePath: 'api/banking',
  AddBankAccount: '',
  RemoveBankAccount: '',
}

bankingPaths = {
  ...bankingPaths,
  AddBankAccount: bankingPaths.BasePath + '/AddBankAccount',
  RemoveBankAccount: bankingPaths.BasePath + '/RemoveBankAccount'
}

let transactionPaths: ApiTransactionPaths = {
  BasePath: 'api/transaction',
  ConfirmIntent: '',
  CreateIntent: '',
  InjectPaymentMethod: '',
  CancelIntent: ''
}

transactionPaths = {
  ...transactionPaths,
  CreateIntent: transactionPaths.BasePath + '/CreateIntent',
  ConfirmIntent: transactionPaths.BasePath + '/ConfirmIntent',
  InjectPaymentMethod: transactionPaths.BasePath + '/InjectPaymentMethod',
  CancelIntent: transactionPaths.BasePath + '/CancelIntent'
}

let ticketPaths: ApiTicketPaths = {
  BasePath: 'api/ticket',
  GetAllUsersTickets: '',
  Verify: ''
}

ticketPaths = {
  ...ticketPaths,
  GetAllUsersTickets: ticketPaths.BasePath + '/users',
  Verify: ticketPaths.BasePath + '/verify'
}

let promoPaths: ApiPromoPaths = {
  BasePath: 'api/promo',
  Update: '',
}

promoPaths = {
  ...promoPaths,
  Update: promoPaths.BasePath + '/update'
}

let popularityPaths: ApiPopularityPaths = {
  BasePath: 'api/popularity',
  Categories: '',
  Events: ''
}

popularityPaths = {
  ...popularityPaths,
  Events: popularityPaths.BasePath + '/events',
  Categories: popularityPaths.BasePath + '/categories'
}

interface ApiAuthPaths
{
  readonly BasePath: string;
  readonly Login: string;
  readonly Authenticate: string;
  readonly UpdatePassword: string;
  readonly ForgotPassword: string;
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
  readonly UpdateAddress: string;
  readonly GetUsersAnalytics: string;
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
  readonly Search: string;
  readonly Explore: string;
  readonly Analytics: string;
  readonly DownVote: string;
}

interface ApiPaymentPaths
{
  readonly BasePath: string;
  readonly AddPaymentMethod: string;
  readonly RemovePaymentMethod: string;
  readonly MakePaymentDefault: string;
}

interface ApiBankingPaths
{
  readonly BasePath: string;
  readonly AddBankAccount: string;
  readonly RemoveBankAccount: string;
}

interface ApiTransactionPaths
{
  readonly BasePath: string;
  readonly CreateIntent: string;
  readonly ConfirmIntent: string;
  readonly InjectPaymentMethod: string;
  readonly CancelIntent: string;
}

interface ApiTicketPaths
{
  readonly BasePath: string;
  readonly GetAllUsersTickets: string;
  readonly Verify: string;
}

interface ApiPromoPaths
{
  readonly BasePath: string;
  readonly Update: string;
}

interface ApiPopularityPaths
{
  readonly BasePath: string;
  readonly Events: string;
  readonly Categories: string;
}

export const UserPaths: ApiUserPaths = userPaths;
export const AuthPaths: ApiAuthPaths = authPaths;
export const EventPaths: ApiEventPaths = eventPaths;
export const PaymentPaths: ApiPaymentPaths = paymentPaths;
export const BankingPaths: ApiBankingPaths = bankingPaths;
export const TransactionPaths: ApiTransactionPaths = transactionPaths;
export const TicketPaths: ApiTicketPaths = ticketPaths;
export const PromoPaths: ApiPromoPaths = promoPaths;
export const PopularityPaths: ApiPopularityPaths = popularityPaths;
