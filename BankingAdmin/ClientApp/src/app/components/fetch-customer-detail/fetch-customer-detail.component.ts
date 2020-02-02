import {Component, OnInit} from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";

import {CustomerData, CustomerService} from '../../services/customer.service';


@Component({
  selector: 'app-fetch-customer-detail',
  templateUrl: './fetch-customer-detail.component.html'
})
export class FetchCustomerDetailComponent implements OnInit{
  customer: CustomerData;
  customerId: number;
  errorMessage: string;

  constructor(private _customerService: CustomerService, private _avRoute: ActivatedRoute,
    private _router: Router) {
    if (_avRoute.snapshot.params["id"])
      this.customerId = _avRoute.snapshot.params["id"];
  }

  ngOnInit() {
    this.refreshCustomer();
  }

  refreshCustomer() {
    this._customerService.getCustomerById(this.customerId).subscribe(data => this.customer = data,
      error => this.errorMessage = error);
  }

  delete() {
    const ans = confirm(`Are you sure you want to delete customer with ID: ${ this.customer.customerId }?`);
    if (ans) {
      this._customerService.deleteCustomer(this.customer.customerId).subscribe(data => {
        this._router.navigate(["/fetch-customer"]);
      })
    }
  }
}  