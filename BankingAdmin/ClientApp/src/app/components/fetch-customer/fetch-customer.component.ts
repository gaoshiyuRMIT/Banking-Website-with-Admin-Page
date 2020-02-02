import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { CustomerService, CustomerData } from '../../services/customer.service';

@Component({
  selector: 'app-fetch-customer',
  templateUrl: './fetch-customer.component.html'
})
export class FetchCustomerComponent {
  customerList: CustomerData[];
  customerIdForm: FormGroup;

  constructor(private _customerService: CustomerService, private _fb: FormBuilder) {
    this.getCustomers();
    this.customerIdForm = _fb.group({
      customerId: [null, [Validators.max(9999), Validators.min(0)]]
    });
  }

  getCustomers() {
    this._customerService.getCustomers().subscribe(data => this.customerList = data,
      error => console.error(error));
  }

  search() {
    if (this.customerId.value !== null)
      this._customerService.getCustomerById(this.customerId.value).subscribe(
        data => {
          console.log(data);
          this.customerList = data !== null ? [data] : [];
        },
        error => console.error(error));
    else
      this.getCustomers();
  }

  get customerId() {
    return this.customerIdForm.get("customerId");
  }
}