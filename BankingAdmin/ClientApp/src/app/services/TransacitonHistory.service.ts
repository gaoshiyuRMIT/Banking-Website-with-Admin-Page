import { Injectable, Inject } from "@angular/core";
import { HttpClient,HttpParams } from "@angular/common/http";



export interface TransactionHistoryData {
  transactionId: number;
  modifyDate: string;
  transactionType: string;
  accountNumber: number;
  destAccountNumber: number;
  amount: number;
  comment: string;

}
@Injectable()
export class transactionHistoryService {
  myAppUrl: string = "";

  constructor(private _http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.myAppUrl = baseUrl;
  }

  TrimTransactionHistoryData(t: TransactionHistoryData): TransactionHistoryData
  {
    return {
      transactionId: t.transactionId,
      modifyDate: t.modifyDate,
      transactionType: t.transactionType,
      accountNumber: t.accountNumber,
      destAccountNumber: t.destAccountNumber,
      amount: t.amount,
      comment: t.comment,
   };
  }

  getTransactionHistory() {
  //params = new HttpParams().set('customerId', 'value');, { params: params }

    const params = new HttpParams().set('Amount', '20');
    return this._http.get<TransactionHistoryData[]>(this.myAppUrl + "api/Transaction",{ params: params })
}

  getTransactionHistoryById(id: number)
  {
    return this._http.get<TransactionHistoryData>(this.myAppUrl + "api/Transaction/" + id);
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

