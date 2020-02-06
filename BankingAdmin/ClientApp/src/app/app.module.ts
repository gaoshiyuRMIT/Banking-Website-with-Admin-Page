import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';

import {CustomerService} from './services/customer.service';
import { FetchCustomerComponent } from './components/fetch-customer/fetch-customer.component';
import {FetchCustomerDetailComponent} from './components/fetch-customer-detail/fetch-customer-detail.component';
import {EditCustomerDetailComponent} from './components/edit-customer-detail/edit-customer-detail.component';
import {FetchBillPayComponent} from './components/fetch-billpay/fetch-billpay.component';
import { BillPayService } from './services/billpay.service';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    FetchCustomerComponent,
    FetchCustomerDetailComponent,
    EditCustomerDetailComponent,
    FetchBillPayComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'fetch-customer', component: FetchCustomerComponent },
      {path: 'fetch-customer-detail/:id', component: FetchCustomerDetailComponent},
      {path: 'edit-customer-detail/:id', component: EditCustomerDetailComponent},
      {path: 'fetch-billpay', component: FetchBillPayComponent},
    ])
  ],
  providers: [CustomerService, BillPayService],
  bootstrap: [AppComponent]
})
export class AppModule { }
