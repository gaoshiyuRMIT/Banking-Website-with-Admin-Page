import { Component } from '@angular/core';
import { Router, ActivatedRoute, Params } from "@angular/router";

import {TransactionData, TransactionService} from "../../services/transaction.service";

@Component({
  selector: 'app-graph-transaction',
  templateUrl: './graph-transaction.component.html'
})
export class GraphTransactionComponent {
  transactionList: TransactionData[];
  errorMessage: string = "";
  params: Params;

  constructor(private _transactionService: TransactionService, private _avRoute: ActivatedRoute,
    private _router: Router)
  {
    _avRoute.queryParams.subscribe(
      params => this._transactionService.getTransactionsByQuery(params).subscribe(
        data => this.transactionList = data,
        error => this.errorMessage += error),
      error => this.errorMessage += error);
  }
}  