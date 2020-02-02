import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';

import {CustomerService} from '../../services/customer.service';

@Component({
  selector: 'app-edit-customer-detail',
  templateUrl: './edit-customer-detail.component.html'
})
export class EditCustomerDetailComponent implements OnInit {
  customerForm: FormGroup;
  customerId: number;
  errorMessage: any;

  constructor(private _customerService: CustomerService, private _avRoute: ActivatedRoute,
    private _router: Router, private _formBuilder: FormBuilder) {
    if (_avRoute.snapshot.params["id"])
      this.customerId = _avRoute.snapshot.params["id"];
    this.customerForm = _formBuilder.group({
      customerId: [null, Validators.required],
      name: ["", Validators.required],
      address: [""],
      state: [""],
      city: [""],
      postCode: ["", [Validators.maxLength(4), Validators.minLength(4)]],
      phone: ["", [Validators.required, Validators.maxLength(9), Validators.minLength(9)]],
      tfn: [""],
    });
  }

  ngOnInit() {
    this._customerService.getCustomerById(this.customerId).subscribe(
      data => {
        this.customerForm.setValue(CustomerService.TrimCustomerData(data));
        console.log(data);
      },
      error => {
        this.errorMessage = error;  
      });
  }

  save() {
    if (!this.customerForm.valid)
      return;
    this._customerService.updateCustomer(this.customerId, this.customerForm.value).subscribe(
      data => this._router.navigate(["/fetch-customer-detail", this.customerId]),
      error => this.errorMessage = error);
    
  }

  get name() {
    return this.customerForm.get("name");
  }

  get address() {
    return this.customerForm.get("address");
  }

  get state() {
    return this.customerForm.get("state");
  }

  get city() {
    return this.customerForm.get("city");
  }

  get postCode() {
    return this.customerForm.get("postCode");
  }

  get phone() {
    return this.customerForm.get("phone");
  }

  get tfn() {
    return this.customerForm.get("tfn");
  }
}
  
  