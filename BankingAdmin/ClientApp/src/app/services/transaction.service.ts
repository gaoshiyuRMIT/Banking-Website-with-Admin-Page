import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import * as moment from "moment";

export interface TransactionData
{
    transactionID: number;
    transactionType: string;
    accountNumber: number;
    destAccountNumber: number;
    amount: number;
    comment: string;
    modifyDate: string;
}


export interface TransactionQueryFormData
{
    transactionId?: number;
    modifyDateFrom?: Date;
    modifyDateTo?: Date;
    transactionType?: string;
    accountNumber?: number;
    amountFrom?: number;
    amountTo?: number;
    commentContains?: string;
}

@Injectable()
export class TransactionService {
  myAppUrl: string = "";
  readonly format: string = "DD/MM/YYYY hh:MM a";

  constructor(private _http: HttpClient, @Inject("BASE_URL") baseUrl: string)
  {
    this.myAppUrl = baseUrl;
  }


  transformTransactionData(td: TransactionData): TransactionData
  {
    td.modifyDate = moment(td.modifyDate, this.format).local().format(this.format);
    return td;
  }

  transformTransactionQuery(tq: TransactionQueryFormData): HttpParams
  {
    let params = new HttpParams();
    if (tq.transactionId)
      params = params.set("transactionId", tq.transactionId + "");
    if (tq.transactionType)
      params = params.set("transactionType", tq.transactionType);
    if (tq.modifyDateFrom)
      params = params.set("modifyDateFrom", moment(tq.modifyDateFrom).utc().format(this.format));
    if (tq.modifyDateTo)
      params = params.set("modifyDateTo", moment(tq.modifyDateTo).utc().format(this.format));
    if (tq.accountNumber)
      params = params.set("accountNumber", tq.accountNumber + "");
    if (tq.amountFrom)
      params = params.set("amountFrom", tq.amountFrom + "");
    if (tq.amountTo)
      params = params.set("amountTo", tq.amountTo + "");
    if (tq.commentContains)
      params = params.set("commentContains", tq.commentContains);
    return params;
  }

  getTransactionsByQuery(tq: TransactionQueryFormData)
  {
    let params = this.transformTransactionQuery(tq);
    return this._http.get<TransactionData[]>(this.myAppUrl + "api/transaction", {params: params});
  }
}