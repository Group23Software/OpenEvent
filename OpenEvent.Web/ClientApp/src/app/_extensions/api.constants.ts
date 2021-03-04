import {tick} from "@angular/core/testing";

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
  GetUsersAnalytics: userPaths.BasePath + '/GetUsersAnalytics'
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
  Explore: ''
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
  Explore: eventPaths.BasePath + '/explore'
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

export const UserPaths: ApiUserPaths = userPaths;
export const AuthPaths: ApiAuthPaths = authPaths;
export const EventPaths: ApiEventPaths = eventPaths;
export const PaymentPaths: ApiPaymentPaths = paymentPaths;
export const BankingPaths: ApiBankingPaths = bankingPaths;
export const TransactionPaths: ApiTransactionPaths = transactionPaths;
export const TicketPaths: ApiTicketPaths = ticketPaths;
