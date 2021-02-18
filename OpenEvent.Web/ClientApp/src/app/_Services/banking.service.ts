import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {UserService} from "./user.service";
import {AddBankAccountBody, BankAccountViewModel, RemoveBankAccountBody} from "../_models/BankAccount";
import {Observable} from "rxjs";
import {BankingPaths} from "../_extensions/api.constants";
import {map} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class BankingService
{
  private readonly BaseUrl: string;

  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private userService: UserService)
  {
    this.BaseUrl = baseUrl;
    this.BaseUrl = 'http://localhost:5000/';
  }

  public AddBankAccount (addBankAccountBody: AddBankAccountBody): Observable<BankAccountViewModel>
  {
    return this.http.post<BankAccountViewModel>(this.BaseUrl + BankingPaths.AddBankAccount, addBankAccountBody).pipe(map(bank =>
    {
      this.userService.User.BankAccounts = [bank];
      return bank;
    }));
  }

  public RemoveBankAccount (removeBankAccountBody: RemoveBankAccountBody): Observable<any>
  {
    return this.http.post(this.BaseUrl + BankingPaths.RemoveBankAccount, removeBankAccountBody).pipe(map(b =>
    {
      this.userService.User.BankAccounts = null;
      return b;
    }));
  }
}
