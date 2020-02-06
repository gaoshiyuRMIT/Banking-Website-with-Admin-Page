import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { transactionHistoryService, TransactionHistoryData } from '../services/transacitonHistory.service';

@Component({
  selector: 'app-transaction-history',
  templateUrl: './transaction-history.component.html'
})
export class transactionHistoryComponent {
  historyList: TransactionHistoryData[];
  transactionHistoryForm: FormGroup;

  constructor(private _transactionHistoryService: transactionHistoryService, private _fb: FormBuilder) {
    this.getTransactionHistory();
    this.transactionHistoryForm = _fb.group({
      customerId: [null, [Validators.max(9999), Validators.min(0)]]
    });
  }


  getTransactionHistory() {
    this._transactionHistoryService.getTransactionHistory().subscribe(data => this.historyList = data,
      error => console.error(error));
  }

//  search() {
//    if (this.customerId.value !== null)
//     this._transactionHistoryService.getCustomerById(this.customerId.value).subscribe(
//        data => {
//          this.historyList = data !== null ? [data] : [];
//       },
//        error => console.error(error));
//    else
//      this.getTransactionHistory();
//  }

//    get transactionId() {
//      return this.transactionHistoryForm.get("transactionId");
//  }
//}
