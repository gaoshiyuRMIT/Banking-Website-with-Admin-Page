import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ValidatorFn, ValidationErrors } from '@angular/forms';
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
    barChart: any;
    lineChart: any;
    pieChart: any;

    constructor(private _transactionService: TransactionService, private _avRoute: ActivatedRoute,
      private _router: Router, private _fb: FormBuilder) {
      
      this._transactionService.getTransactionsByQuery({}).subscribe(
        data => {
          this.transactionList = data;
          this.transactionList.forEach((val, idx, arr) => 
            arr[idx] = this._transactionService.transformTransactionData(val)); 
          this.createCharts();
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
      }, {validators: [amountRangeValidator, modDateRangeValidator]});
    }

    showTable() {
        this.option = FetchTransactionOption.list;
    }

    showBar() {
        this.option = FetchTransactionOption.barChart;
        this.updateBarChart();
    }

    showLine() {
        this.option = FetchTransactionOption.lineChart;
    }

    showPie() {
        this.option = FetchTransactionOption.pieChart;
    }

    get displayList(): boolean {
      return this.option === FetchTransactionOption.list;
    }

    get displayBar() {
      return this.option === FetchTransactionOption.barChart;
    }

    get displayLine() {
      return this.option === FetchTransactionOption.lineChart;
    }

    get displayPie() {
      return this.option === FetchTransactionOption.pieChart;
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

    updatePieChart() {
      const t2c = this._transactionService.getTypeToCount(this.transactionList);
      const data = t2c.map(v => v.count);
      this.pieChart.data.datasets.forEach(dataset => {
        dataset.data = data;
      });
      this.pieChart.update();
    }

    createPieChart() {
      // type to count
      const canvas = document.getElementById("pieChart");
      const t2c = this._transactionService.getTypeToCount(this.transactionList);
      const data = t2c.map(v => v.count);
      const labels = t2c.map(v => v.type);
      this.pieChart = new Chart(canvas, {
        type: 'pie',
        data: {
          labels: labels,
          datasets: [{
            label: "Transaction Count by Type",
            data: data,
            backgroundColor: [
              "rgba(255, 157, 0, 0.5)",
              "rgba(255, 208, 48, 0.5)",
              "rgba(0, 196, 195, 0.5)",
              "rgba(0, 158, 242, 0.5)",
              "rgba(255, 77, 125, 0.5)"
            ]
          }]
        }
      });
    }

    updateLineChart() {
      const d2a = this._transactionService.getDateToTotalAmount(this.transactionList);
      const data = d2a.map(v => {
        return {t: v.date, y: v.amount};
      });
      this.lineChart.data.datasets.forEach(dataset => {
        dataset.data = data;
      });
      this.lineChart.update();
    }

    createLineChart() {
      // date to total amount
      const canvas = document.getElementById("lineChart");
      const d2a = this._transactionService.getDateToTotalAmount(this.transactionList);
      const data = d2a.map(v => {
        return {t: v.date, y: v.amount};
      });
      const labels = d2a.map(v => v.dateLabel);
      this.lineChart = new Chart(canvas, {
        type: "line",
        data: {
          labels: labels,
          datasets: [{
            label: "Transaction Amount",
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
            backgroundColor: "rgba(0, 183, 0, 0.2)",
            borderColor: "rgba(0, 183, 0, 1)",
            borderWidth: 1
          }]
        }
      });

    }

    updateBarChart() {
      const d2c = this._transactionService.getDateToCount(this.transactionList);
      const data = d2c.map(val => {return {t: val.date, y: val.count};});
      this.barChart.data.datasets.forEach((dataset) => dataset.data = data);
      this.barChart.update();
    }

    createBarChart() {
      // date to count
      let canvas = document.getElementById("barChart");
      const d2c = this._transactionService.getDateToCount(this.transactionList);
      const data = [];
      const labels = [];
      for(let val of d2c) {
        labels.push(val.dateLabel);
        data.push({t: val.date, y: val.count});
      }
      this.barChart = new Chart(canvas, {
        type: "bar",
        data: {
          labels: labels,
          datasets: [{
            label: "Transaction Count",
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
        },
        options: {
          responsive: true
        }
      });
    }

    search() {
      if (!this.queryForm.valid)
        return;
      this._transactionService.getTransactionsByQuery(this.queryForm.value).subscribe(
        data => {
          this.transactionList = data;
          this.transactionList.forEach((val, idx, arr) => 
            arr[idx] = this._transactionService.transformTransactionData(val)); 
          this.updateCharts();
        },
        error => console.error(error)
      );
    }

    createCharts() {
      this.createBarChart();
      this.createLineChart();
      this.createPieChart();
    }

    updateCharts() {
      this.updateBarChart();
      this.updateLineChart();
      this.updatePieChart();
    }
  }

export enum FetchTransactionOption {
  list = "list", 
  pieChart = "pie", 
  lineChart = "line", 
  barChart = "bar"
}

export const amountRangeValidator: ValidatorFn = (control: FormGroup): ValidationErrors | null => {
  const amountFrom = control.get('amountFrom').value;
  const amountTo = control.get('amountTo').value;
  return amountFrom && amountTo && amountFrom > amountTo ? { 'amountRange': true } : null;
};

export const modDateRangeValidator: ValidatorFn = (control: FormGroup): ValidationErrors | null => {
  const dateFrom = control.get("modifyDateFrom").value;
  const dateTo = control.get("modifyDateTo").value;
  return dateFrom && dateTo && dateFrom > dateTo ? {'modDateRange': true} : null;
}