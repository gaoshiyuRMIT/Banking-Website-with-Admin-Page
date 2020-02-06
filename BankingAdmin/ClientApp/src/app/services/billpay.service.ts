import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";

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

  constructor(private _http: HttpClient, @Inject("BASE_URL") baseUrl: string)
  {
    this.myAppUrl = baseUrl;
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