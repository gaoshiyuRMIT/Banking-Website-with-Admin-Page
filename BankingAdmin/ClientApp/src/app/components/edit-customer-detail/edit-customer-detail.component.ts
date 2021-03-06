import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';

import {CustomerService, ausStateValidator} from '../../services/customer.service';

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
      customerID: [null, Validators.required],
      name: ["", [Validators.required, Validators.pattern(String.raw`^[a-zA-Z ]+$`)]],
      address: [""],
      state: ["", ausStateValidator()],
      city: [""],
      postCode: ["", [Validators.pattern(String.raw`^\d{4}$`)]],
      phone: ["", [Validators.required, Validators.pattern(String.raw`^\d{9}$`)]],
      tfn: ["", Validators.maxLength(11)],
    }, {updateOn: "submit"});
  }

  ngOnInit() {
    this._customerService.getCustomerById(this.customerId).subscribe(
      data => {
        this.customerForm.setValue(this._customerService.TrimCustomerData(data));
      },
      error => {
        this.errorMessage = error;  
      });
  }

  save() {
    if (!this.customerForm.valid)
      return;
    this.name.setValue(this.name.value || null);
    this.address.setValue(this.address.value || null);
    this.state.setValue(this.state.value || null);
    this.city.setValue(this.city.value || null);
    this.postCode.setValue(this.postCode.value || null);
    this.phone.setValue(this.phone.value || null);
    this.tfn.setValue(this.tfn.value || null);

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
  
  