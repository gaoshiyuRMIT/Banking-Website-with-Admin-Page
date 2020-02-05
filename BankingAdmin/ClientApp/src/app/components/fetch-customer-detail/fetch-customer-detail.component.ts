import {Component, OnInit} from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";

import {CustomerData, CustomerService, CustomerDataWLogin} from '../../services/customer.service';


@Component({
  selector: 'app-fetch-customer-detail',
  templateUrl: './fetch-customer-detail.component.html'
})
export class FetchCustomerDetailComponent implements OnInit{
  customer: CustomerDataWLogin;
  customerId: number;
  errorMessage: string = "";

  constructor(private _customerService: CustomerService, private _avRoute: ActivatedRoute,
    private _router: Router) {
    if (_avRoute.snapshot.params["id"])
      this.customerId = _avRoute.snapshot.params["id"];
  }

  ngOnInit() {
    this.refreshCustomer();
  }

  refreshCustomer() {
    this._customerService.getCustomerById(this.customerId).subscribe(
      data => {
        this._customerService.getLockedStatusById(this.customerId).subscribe(
          locked => this.customer = this._customerService.ConstructCustomerDataWLogin(data, locked),
          error => this.errorMessage += error
        )
      },
      error => this.errorMessage += error);
  }

  delete() {
    const ans = confirm(`Are you sure you want to delete customer with ID: ${ this.customer.customerID }?`);
    if (ans) {
      this._customerService.deleteCustomer(this.customer.customerID).subscribe(data => {
        this._router.navigate(["/fetch-customer"]);
      })
    }
  }

  lock() {
    this._customerService.lockById(this.customer.customerID).subscribe(data => {
      this.refreshCustomer();
    });
  }

  unlock() {
    if (this.customer.locked) {
      this._customerService.unlockById(this.customer.customerID).subscribe(data => {
        this.refreshCustomer();
      });
    }
  }
}  