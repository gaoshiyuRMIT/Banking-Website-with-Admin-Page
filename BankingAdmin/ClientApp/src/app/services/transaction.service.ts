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

export interface TransactionDateToCount
{
  date: Date;
  dateLabel: string;
  count: number;
}

export interface TransactionDateToTotalAmount
{
  date: Date;
  dateLabel: string;
  amount: number;
}

export interface TransactionTypeToCount
{
  type: string;
  count: number;
}

@Injectable()
export class TransactionService {
  myAppUrl: string = "";
  readonly format: string = "DD/MM/YYYY hh:mm:ss A";

  constructor(private _http: HttpClient, @Inject("BASE_URL") baseUrl: string)
  {
    this.myAppUrl = baseUrl;
  }


  transformTransactionData(td: TransactionData): TransactionData
  {
    td.modifyDate = moment.utc(td.modifyDate, this.format).local().format(this.format);
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

  getTypeToCount(tdList: TransactionData[]): TransactionTypeToCount[] 
  {
    if (tdList.length === 0)
      return [];
      let counter = new Map<string, number>();
      const types = ["Deposit", "Withdrawal", "Transfer", "BillPay", "ServiceCharge"];
      for (let transaction of tdList) {
        const type = transaction.transactionType;
        counter.set(type, (counter.get(type) || 0) + 1);
      }
      const result: TransactionTypeToCount[] = [];
      for (const tp of types) {
        result.push({type: tp, count: counter.get(tp) || 0});
      }
      return result;
    }

  getDateToTotalAmount(tdList: TransactionData[]): TransactionDateToTotalAmount[] 
  {
    if (tdList.length === 0)
      return [];
    let counter = new Map<string, number>();
    let sortedList = this.sortTransactionByModifyDate(tdList);
    for (let transaction of sortedList) {
      let dateS = this.getDateStringPrecisionDay(transaction.modifyDate);
      const amount = transaction.amount;
      counter.set(dateS, (counter.get(dateS) || 0) + amount);
    }
    let earliest = this.getMomentPrecisionDay(moment(sortedList[0].modifyDate, this.format));
    let result: TransactionDateToTotalAmount[] = [];
    let today = moment();
    for (let dt = earliest; dt <= today; dt = dt.add(1, 'd'))
    {
      result.push({
        dateLabel: dt.format("DD/MM/YYYY"), 
        date: dt.toDate(), 
        amount: counter.get(dt.format(this.format)) || 0});
    }
    return result;  
  }

  getDateToCount(tdList: TransactionData[]): TransactionDateToCount[]
  {
    if (tdList.length === 0)
      return [];
    let counter = new Map<string, number>();
    let sortedList = this.sortTransactionByModifyDate(tdList);
    for (let transaction of sortedList) {
      let dateS = this.getDateStringPrecisionDay(transaction.modifyDate);
      counter.set(dateS, (counter.get(dateS) || 0) + 1);
    }
    let earliest = this.getMomentPrecisionDay(moment(sortedList[0].modifyDate, this.format));
    let result: TransactionDateToCount[] = [];
    let today = moment();
    for (let dt = earliest; dt <= today; dt = dt.add(1, 'd'))
    {
      result.push({
        dateLabel: dt.format("DD/MM/YYYY"), 
        date: dt.toDate(), 
        count: counter.get(dt.format(this.format)) || 0});
    }
    return result;
  }

  getMomentPrecisionDay(dt: moment.Moment): moment.Moment {
    return dt.hours(0).minutes(0).seconds(0);
  }

  getDateStringPrecisionDay(dts: string): string {
    return moment(dts, this.format).hours(0).minutes(0).seconds(0)
      .format(this.format);
  }

  sortTransactionByModifyDate(tdList: TransactionData[]): TransactionData[] {
    return tdList.concat().sort((tr1, tr2) => {
      const t1 = +moment(tr1.modifyDate, this.format);
      const t2 = +moment(tr2.modifyDate, this.format);
      return t1 - t2;
    });
  }

  getTransactionsByQuery(tq: TransactionQueryFormData)
  {
    let params = this.transformTransactionQuery(tq);
    return this._http.get<TransactionData[]>(this.myAppUrl + "api/transaction", {params: params});
  }
}