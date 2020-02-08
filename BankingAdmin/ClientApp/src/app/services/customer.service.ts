import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import {ValidatorFn, AbstractControl} from '@angular/forms';

export interface CustomerData {
    customerID: number;
    name: string;
    address: string;
    city: string;
    state: string;
    postCode: string;
    phone: string;
    tfn: string;
}

export interface CustomerDataWLogin extends CustomerData
{
  locked: boolean
}

@Injectable()
export class CustomerService {
  myAppUrl: string = "";

  constructor(private _http: HttpClient, @Inject("BASE_URL") baseUrl: string)
  {
    this.myAppUrl = baseUrl;
  }

  TrimCustomerData(c: CustomerData): CustomerData
  {
    return {
      customerID: c.customerID,
      name : c.name,
      address : c.address,
      city : c.city,
      state : c.state,
      postCode : c.postCode,
      phone : c.phone,
      tfn : c.tfn
    };
  }

  ConstructCustomerDataWLogin(c: CustomerData, locked: boolean): CustomerDataWLogin
  {
    return {
      customerID: c.customerID,
      name : c.name,
      address : c.address,
      city : c.city,
      state : c.state,
      postCode : c.postCode,
      phone : c.phone,
      tfn : c.tfn,
      locked: locked
    };
  }

  getCustomers()
  {
    return this._http.get<CustomerData[]>(this.myAppUrl + "api/Customer",);
  }

  getCustomerById(id: number)
  {
    return this._http.get<CustomerData>(this.myAppUrl + "api/Customer/" + id);
  }

  getLockedStatusById(id: number)
  {
    return this._http.get<boolean>(this.myAppUrl + "api/login/lock/customer/" + id);
  }

  lockById(id: number)
  {
    return this._http.put(this.myAppUrl + "api/login/lock/customer/" + id, null);
  }

  unlockById(id: number)
  {
    return this._http.put(this.myAppUrl + "api/login/unlock/customer/" + id, null);
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

export function ausStateValidator(): ValidatorFn {
  return (control: AbstractControl): {[key: string]: any} | null => {
    if (control.value && !["NSW", "QLD", "SA", "TAS", "VIC", "WA", "ACT", "NT"].includes(control.value))
      return {'ausState': {value: control.value}};
    return null;
  };
}