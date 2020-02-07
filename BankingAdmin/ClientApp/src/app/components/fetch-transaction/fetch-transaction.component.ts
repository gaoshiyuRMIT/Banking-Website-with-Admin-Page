import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpParams } from '@angular/common/http';

import { TransactionData, TransactionService } from '../../services/transaction.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-fetch-transaction',
  templateUrl: './fetch-transaction.component.html'
})
export class FetchTransactionComponent {
    transactionList: TransactionData[];
    queryForm: FormGroup;
    option: FetchCustomerOption = FetchCustomerOption.list;

    constructor(private _transactionService: TransactionService, private _avRoute: ActivatedRoute,
      private _router: Router, private _fb: FormBuilder) {
      
      this._transactionService.getTransactionsByQuery({}).subscribe(
        data => this.transactionList = data,
        error => console.error(error));
      this.queryForm = _fb.group({
        transactionId: [null],
        modifyDateFrom: [null],
        modifyDateTo: [null],
        transactionType: [null],
        accountNumber: [null, [Validators.min(0), Validators.max(9999)]],
        amountFrom: [null, Validators.min(0)],
        amountTo: [null, Validators.min(0)],
        commentContains: [null]
      });
    }
  }

export enum FetchCustomerOption {
  list = "list", 
  pieChart = "pie", 
  lineChart = "line", 
  barChart = "bar"
}