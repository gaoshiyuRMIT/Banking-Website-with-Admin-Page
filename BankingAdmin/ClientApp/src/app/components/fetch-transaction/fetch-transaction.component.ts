import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpParams } from '@angular/common/http';
import {Chart} from 'chart.js';

import { TransactionData, TransactionService } from '../../services/transaction.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-fetch-transaction',
  templateUrl: './fetch-transaction.component.html'
})
export class FetchTransactionComponent {
    transactionList: TransactionData[];
    queryForm: FormGroup;
    option: FetchTransactionOption = FetchTransactionOption.list;

    constructor(private _transactionService: TransactionService, private _avRoute: ActivatedRoute,
      private _router: Router, private _fb: FormBuilder) {
      
      this._transactionService.getTransactionsByQuery({}).subscribe(
        data => {
          this.transactionList = data;
          this.transactionList.forEach((val, idx, arr) => 
            arr[idx] = this._transactionService.transformTransactionData(val)); 
          this.createBarChart();

        },
        error => console.error(error));
      this.queryForm = this.createFormGroup();
    }

    createFormGroup(): FormGroup {
      return this._fb.group({
        transactionId: [null, [Validators.min(0), Validators.max(9999)]],
        modifyDateFrom: [null],
        modifyDateTo: [null],
        transactionType: [null],
        accountNumber: [null, [Validators.min(0), Validators.max(9999)]],
        amountFrom: [null, Validators.min(0)],
        amountTo: [null, Validators.min(0)],
        commentContains: [null]
      });
    }

    get displayList(): boolean {
      // return this.option === FetchTransactionOption.list;
      return true;
    }

    get displayBar() {
      return true;
    }

    get displayLine() {
      return true;
    }

    get displayPie() {
      return true;
    }

    get transactionId() {
      return this.queryForm.get("transactionId");
    }

    get modifyDateFrom() {
      return this.queryForm.get("modifyDateFrom");
    }

    get modifyDateTo() {
      return this.queryForm.get("modifyDateTo");
    }

    get transactionType() {
      return this.queryForm.get("transactionType");
    }

    get accountNumber() {
      return this.queryForm.get("accountNumber");
    }

    get amountFrom() {
      return this.queryForm.get("amountFrom");
    }

    get amountTo() {
      return this.queryForm.get("amountTo");
    }

    get commentContains() {
      return this.queryForm.get("commentContains");
    }

    createBarChart() {
      // date to count
      let canvas = document.getElementById("barChart");
      const d2c = this._transactionService.getDateToCount(this.transactionList);
      console.log(d2c);
      const data = [];
      const labels = [];
      for(let val of d2c) {
        labels.push(val.dateLabel);
        data.push({t: val.date, y: val.count});
      }
      new Chart(canvas, {
        type: "bar",
        data: {
          labels: labels,
          datasets: [{
            label: "Transaction count",
            data: data,
            options: {
              scales: {
                xAxes: [{
                  type: "time",
                  time: {
                    unit: "day"
                  }
                }]
              }
            },
            backgroundColor: "rgba(255, 99, 132, 0.2)",
            borderColor: "rgba(255, 99, 132, 1)",
            borderWidth: 1
          }]
        }
      });
    }

    search() {
      this._transactionService.getTransactionsByQuery(this.queryForm.value).subscribe(
        data => {
          this.transactionList = data;
          this.transactionList.forEach((val, idx, arr) => 
            arr[idx] = this._transactionService.transformTransactionData(val)); 
          this.createBarChart();
        },
        error => console.error(error)
      );
    }
  }

export enum FetchTransactionOption {
  list = "list", 
  pieChart = "pie", 
  lineChart = "line", 
  barChart = "bar"
}