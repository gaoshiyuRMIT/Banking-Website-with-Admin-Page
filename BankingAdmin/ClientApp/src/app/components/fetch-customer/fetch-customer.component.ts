import { Component } from '@angular/core';
import { CustomerService, CustomerData } from '../../services/customer.service';

@Component({
  selector: 'app-fetch-customer',
  templateUrl: './fetch-customer.component.html'
})
export class FetchCustomerComponent {
  customerList: CustomerData[];

  constructor(private _customerService: CustomerService) {
    this.getCustomers();
  }

  getCustomers() {
    this._customerService.getCustomers().subscribe(data => this.customerList = data,
      error => console.error(error));
  }
}