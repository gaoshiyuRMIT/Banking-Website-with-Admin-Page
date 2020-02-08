import { Component } from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import { LoginService } from '../services/login.service';



@Component({
  selector: 'app-home',
})
export class HomeComponent {
  constructor(private _router: Router, private _loginService: LoginService) {
    if (_loginService.isLoggedIn)
      _router.navigate(['/fetch-customer']);
    else
      _router.navigate(["/admin-login"]);
  }
}
