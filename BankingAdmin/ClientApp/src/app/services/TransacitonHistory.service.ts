import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";



export interface TransactionHistoryData {
  TransactionId: number;
  ModifyDate: number;
  TransactionType: number;
  AccountNumber: number;
  DestAccountNumber: number;
  Amount: number;
  Comment: string;

}
@Injectable()
export class transactionHistoryService {
  myAppUrl: string = "";

  constructor(private _http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.myAppUrl = baseUrl;
  }

  TrimTransactionHistoryData(t: TransactionHistoryData): TransactionHistoryData {
    return {
      TransactionId: t.TransactionId,
      ModifyDate: t.ModifyDate,
      TransactionType: t.TransactionType,
      AccountNumber: t.AccountNumber,
      DestAccountNumber: t.DestAccountNumber,
      Amount: t.Amount,
      Comment: t.Comment,
   };
  }

  getTransactionHistory() {
  params = new HttpParams().set('customerId', 'value');
    return this._http.get<TransactionHistoryData[]>(this.myAppUrl + "api/TransactionHistory", { params: params })
}

  //   getTransactionHistory() {
  //  return this._http.get<TransactionHistoryData[]>(this.myAppUrl + "api/");
  //}
  
  //getTransactionHistoryById(id: number) {
    //return this._http.get<TransactionHistoryData>(this.myAppUrl + "api/TransactionHistory/" + id);
  //}

  //updateCustomer(id: number, customer) {
  //  return this._http.put(this.myAppUrl + "api/Customer/Edit/" + id, customer);
  //}

  //deleteCustomer(id: number) {
  //  return this._http.delete(this.myAppUrl + "api/Customer/Delete/" + id);
  //}
}

