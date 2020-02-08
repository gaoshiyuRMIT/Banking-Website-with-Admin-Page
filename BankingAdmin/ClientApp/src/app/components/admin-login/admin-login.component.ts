import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';

import {LoginService} from '../../services/login.service';

@Component({
  selector: 'app-admin-login',
  templateUrl: './admin-login.component.html'
})
export class AdminLoginComponent {
    loginForm: FormGroup;
    errorMessage: string;

    constructor(private _loginService: LoginService, private _avRoute: ActivatedRoute,
      private _router: Router, private _fb: FormBuilder) {
      
      this.loginForm = _fb.group({
        userName: ["", Validators.required],
        password: ["", Validators.required]
      });
    }

    get userName() {
      return this.loginForm.get("userName");
    }

    get password() {
      return this.loginForm.get("password");
    }
    
    submit() {
      this._loginService.login(this.userName.value, this.password.value).subscribe(
        success => {
          if (success) {
            this._loginService.loginSuccess(this.userName.value);
            this._router.navigate(["/fetch-customer"]);
          }
          else {
            this.errorMessage = "Login failed. Please check your credentials.";
            this.loginForm.reset();
          }
        },
        error => console.error(error)
      )
    }
}