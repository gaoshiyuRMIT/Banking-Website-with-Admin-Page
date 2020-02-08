import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';

import {CustomerService} from './services/customer.service';
import { FetchCustomerComponent } from './components/fetch-customer/fetch-customer.component';
import {FetchCustomerDetailComponent} from './components/fetch-customer-detail/fetch-customer-detail.component';
import {EditCustomerDetailComponent} from './components/edit-customer-detail/edit-customer-detail.component';
import {FetchBillPayComponent} from './components/fetch-billpay/fetch-billpay.component';
import { BillPayService } from './services/billpay.service';
import {TransactionService} from './services/transaction.service';
import {FetchTransactionComponent} from './components/fetch-transaction/fetch-transaction.component';
import {LoginService, AuthGuard} from "./services/login.service";
import {AdminLoginComponent} from "./components/admin-login/admin-login.component";


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FetchCustomerComponent,
    FetchCustomerDetailComponent,
    EditCustomerDetailComponent,
    FetchBillPayComponent,
    FetchTransactionComponent,
    AdminLoginComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'fetch-customer', component: FetchCustomerComponent, canActivate : [AuthGuard] },
      {path: 'fetch-customer-detail/:id', component: FetchCustomerDetailComponent,canActivate : [AuthGuard]},
      {path: 'edit-customer-detail/:id', component: EditCustomerDetailComponent,canActivate : [AuthGuard]},
      {path: 'fetch-billpay', component: FetchBillPayComponent,canActivate : [AuthGuard]},
      {path: 'fetch-transaction', component: FetchTransactionComponent,canActivate : [AuthGuard]},
      {path: 'admin-login', component: AdminLoginComponent},
    ])
  ],
  providers: [CustomerService, BillPayService, TransactionService, LoginService, AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
