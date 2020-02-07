import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import * as moment from "moment";


export interface BillPayData {
    billPayID: number;
    accountNumber: number;
    amount: number;
    comment: string;
    modifyDate: string;
    payee: PayeeData;
    period: string;
    scheduleDate: string;
    status: string;
    statusModifyDate: string;
}

export interface PayeeData {
    payeeID: number;
    address: string;
    city: string;
    name: string;
    phone: string;
    postCode: string;
    state: string;
}

@Injectable()
export class BillPayService {
  myAppUrl: string = "";
  readonly format: string = "DD/MM/YYYY hh:mm:ss A";

  constructor(private _http: HttpClient, @Inject("BASE_URL") baseUrl: string)
  {
    this.myAppUrl = baseUrl;
  }

  transformBillPayData(bp: BillPayData): BillPayData 
  {
    bp.scheduleDate = this.transformDateStr(bp.scheduleDate);
    bp.statusModifyDate = this.transformDateStr(bp.statusModifyDate);
    return bp;
  }

  transformDateStr(dts: string): string {
    return moment.utc(dts, this.format).local().format(this.format);
  }

  getBillPays()
  {
      return this._http.get<BillPayData[]>(this.myAppUrl + "api/billpay");
  }

  getBillPayById(id: number)
  {
      return this._http.get<BillPayData>(this.myAppUrl + "api/billpay/" + id);
  }

  blockById(id: number)
  {
      return this._http.put(this.myAppUrl + "api/billpay/" + id + "/block", null);
  }

  unblockById(id: number)
  {
      return this._http.put(this.myAppUrl + "api/billpay/" + id + "/unblock", null);
  }
}