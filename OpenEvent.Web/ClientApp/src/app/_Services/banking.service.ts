import {Inject, Injectable} from '@angular/core';
import {HttpBackend, HttpClient, HttpHeaders} from "@angular/common/http";
import {UserService} from "./user.service";
import {AddBankAccountBody, Balance, BankAccountViewModel, RemoveBankAccountBody} from "../_models/BankAccount";
import {Observable, of} from "rxjs";
import {BankingPaths} from "../_extensions/api.constants";
import {map} from "rxjs/operators";
import {environment} from "../../environments/environment";

@Injectable()
class Service
{


  constructor ()
  {

  }
}

@Injectable({
  providedIn: 'root'
})
export class BankingService
{
  private http: HttpClient;
  private readonly BaseUrl: string;

  constructor (backend: HttpBackend, @Inject('BASE_URL') baseUrl: string, private userService: UserService)
  {
    this.BaseUrl = baseUrl;
    this.BaseUrl = 'http://localhost:5000/';
    this.http = new HttpClient(backend);
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

  public UploadIdentityDocument (fileContent): Observable<any>
  {
    const formData = new FormData();
    formData.append('purpose', 'additional_verification');
    formData.append('file', fileContent);

    return this.http.post<any>('https://files.stripe.com/v1/files', formData, {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + environment.StripeAPIKey
      })
    });
  }

  public AttachFrontFile (fileId: string): Observable<any>
  {
    return this.http.post('https://api.stripe.com/v1/accounts/' + this.userService.User.StripeAccountId + '/persons/' + this.userService.User.StripeAccountInfo.PersonId,
      "verification[document][front]=" + fileId, {
        headers: new HttpHeaders({
          Authorization: 'Bearer ' + environment.StripeAPIKey,
          "content-type": 'application/x-www-form-urlencoded'
        })
      });
  }

  public AttachAdditionalFile (fileId: string): Observable<any>
  {
    return this.http.post('https://api.stripe.com/v1/accounts/' + this.userService.User.StripeAccountId + '/persons/' + this.userService.User.StripeAccountInfo.PersonId,
      "verification[additional_document][front]=" + fileId, {
        headers: new HttpHeaders({
          Authorization: 'Bearer ' + environment.StripeAPIKey,
          "content-type": 'application/x-www-form-urlencoded'
        })
      });
  }


  // readFileContent (file: File): Promise<string>
  // {
  //   return new Promise<string>((resolve, reject) =>
  //   {
  //     if (!file)
  //     {
  //       resolve('');
  //     }
  //
  //     const reader = new FileReader();
  //
  //     reader.onload = (e) =>
  //     {
  //       const text = reader.result.toString();
  //       resolve(text);
  //
  //     };
  //
  //     reader.readAsText(file);
  //   });
  // }

  public GetBalance (): Observable<Balance>
  {
    return this.http.get<Balance>('https://api.stripe.com/v1/balance', {
      headers: new HttpHeaders({
        Authorization: environment.StripeAPIKey
      }).set('Stripe-Account', this.userService.User?.StripeAccountId)
    });
  }
}
