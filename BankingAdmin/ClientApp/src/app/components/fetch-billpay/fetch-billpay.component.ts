import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { BillPayService, BillPayData, PayeeData } from '../../services/billpay.service';

@Component({
  selector: 'app-fetch-billpay',
  templateUrl: './fetch-billpay.component.html'
})
export class FetchBillPayComponent {
    billpayList: BillPayData[];

    constructor(private _billpayService: BillPayService, private _fb: FormBuilder) 
    {
        this.getBillPays();
    }

    getBillPays()
    {
        this._billpayService.getBillPays().subscribe(
            data => {
              this.billpayList = data;
              this.billpayList.forEach((val, idx, arr) =>
                arr[idx] = this._billpayService.transformBillPayData(val));
            },
            error => console.error(error));
    }

    block(id: number)
    {
        let index = this.billpayList.findIndex(x => x.billPayID === id);
        this._billpayService.blockById(id).subscribe(
            data => {this.billpayList[index] = this._billpayService.transformBillPayData(data as BillPayData);},
            error => console.error(error));
    }

    unblock(id: number)
    {
        let index = this.billpayList.findIndex(x => x.billPayID === id);
        this._billpayService.unblockById(id).subscribe(
            data => {this.billpayList[index] = this._billpayService.transformBillPayData(data as BillPayData);},
            error => console.error(error));
    }
}