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
    this._customerService.getCustomerById(this.customerId).subscribe(data => this.customer = data,
      error => this.errorMessage = error);
  }
}  