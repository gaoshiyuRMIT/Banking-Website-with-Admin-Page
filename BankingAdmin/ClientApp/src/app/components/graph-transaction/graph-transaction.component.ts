import { Component } from '@angular/core';
import { TransactionService} from ''

@Component({
  selector: 'app-graph-transaction',
  templateUrl: './graph-transaction.component.html'
})
export class GraphTransactionComponent {
  transactionList: TransactionData[];
}  