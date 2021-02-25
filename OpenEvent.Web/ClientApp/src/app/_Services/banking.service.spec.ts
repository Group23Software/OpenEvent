// import {TestBed} from '@angular/core/testing';
//
// import {BankingService} from './banking.service';
// import {HttpBackend, HttpClient, HttpHandler, HttpResponse} from "@angular/common/http";
// import {of} from "rxjs";
// import {AddBankAccountBody, Balance, BankAccountViewModel, RemoveBankAccountBody} from "../_models/BankAccount";
// import {UserAccountModel} from "../_models/User";
// import {UserService} from "./user.service";
// import {HttpClientTestingModule} from "@angular/common/http/testing";
//
// class FakeUserService
// {
//   set User (value: UserAccountModel)
//   {
//     this._User = value;
//   }
//
//   get User (): UserAccountModel
//   {
//     return this._User;
//   }
//
//   private _User: UserAccountModel = {
//     Avatar: "", Id: "", IsDarkMode: false, UserName: "", PaymentMethods: [], BankAccounts: []
//   };
// }
//
// describe('BankingService', () =>
// {
//   let service: BankingService;
//
//   let userService;
//
//   let httpClientMock;
//   let httpBackendMock;
//
//   const mockAddBankAccountBody: AddBankAccountBody = {BankToken: "BankToken", UserId: "UserId"};
//
//   const mockRemoveBankAccountBody: RemoveBankAccountBody = {BankId: "BankId", UserId: "UserId"};
//
//   const mockBalance: Balance = {available: [], livemode: false, object: "", pending: []}
//
//   const mockBankAccountViewModel: BankAccountViewModel = {
//     Bank: "",
//     Country: "",
//     Currency: "",
//     LastFour: "",
//     Name: "",
//     StripeBankAccountId: ""
//   }
//
//   beforeEach(() =>
//   {
//
//     httpClientMock = jasmine.createSpyObj('HttpClient', ['post','get']);
//
//     httpBackendMock = jasmine.createSpyObj('HttpBackend', ['']);
//
//     TestBed.configureTestingModule({
//       // imports: [HttpClientTestingModule],
//       providers: [
//         {provide: 'BASE_URL', useValue: ''},
//         {provide: HttpClient, useValue: httpClientMock},
//         {provide: HttpBackend, useValue: httpBackendMock},
//         {provide: UserService, useClass: FakeUserService},
//         HttpHandler
//       ]
//     });
//     service = TestBed.inject(BankingService);
//     userService = TestBed.inject(UserService);
//   });
//
//   it('should be created', () =>
//   {
//     expect(service).toBeTruthy();
//   });
//
//   it('should add bank account', () =>
//   {
//     httpClientMock.post.and.returnValue(of(mockBankAccountViewModel));
//     service.AddBankAccount(mockAddBankAccountBody).subscribe(r =>
//     {
//       expect(r).toEqual(mockBankAccountViewModel);
//       expect(userService.User.BankAccounts).toEqual([mockBankAccountViewModel]);
//     });
//   });
//
//   it('should remove bank account', () =>
//   {
//     httpClientMock.post.and.returnValue(of(new HttpResponse({status:200})));
//     service.RemoveBankAccount(mockRemoveBankAccountBody).subscribe(r => {
//       expect(r).toEqual(new HttpResponse({status:200}));
//       expect(userService.User.BankAccounts).toBeNull();
//     });
//   });
//
//   it('should get balance', () =>
//   {
//     httpClientMock.get.and.returnValue(of(mockBalance));
//     service.GetBalance().subscribe(r => {
//       expect(r).toEqual(mockBalance);
//     });
//   });
//
//
// });
