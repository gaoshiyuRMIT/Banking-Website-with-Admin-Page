import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";

export interface CustomerData {
    customerId: number;
    name: string;
    address: string;
    city: string;
    state: string;
    postCode: string;
    phone: string;
    Tfn: string;
}

@Injectable()
export class CustomerService {
  myAppUrl: string = "";

  constructor(private _http: HttpClient, @Inject("BASE_URL") baseUrl: string)
  {
    this.myAppUrl = baseUrl;
  }

  getCustomers()
  {
    return this._http.get<CustomerData[]>(this.myAppUrl + "api/Customer");
  }

  getCustomerById(id: number)
  {
    return this._http.get<CustomerData>(this.myAppUrl + "api/Customer/" + id);
  }

  updateCustomer(id: number, customer)
  {
    return this._http.put(this.myAppUrl + "api/Customer/Edit/" + id, customer);
  }

  deleteCustomer(id: number)
  {
    return this._http.delete(this.myAppUrl + "api/Customer/Delete/" + id);
  }
}