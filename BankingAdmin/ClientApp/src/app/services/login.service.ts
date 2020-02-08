import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ActivatedRouteSnapshot, RouterStateSnapshot, CanActivate, Router } from '@angular/router';

@Injectable()
export class LoginService {
  myAppUrl: string = "";

  constructor(private _http: HttpClient, @Inject("BASE_URL") baseUrl: string)
  {
    this.myAppUrl = baseUrl;
  }

  login(userName: string, password: string) {
    return this._http.get<boolean>(this.myAppUrl + "api/login/admin", {params: {
        userName: userName, 
        password: password
    }});
  }

  loginSuccess(userName: string) {
    localStorage.setItem("user", userName);
  }

  logout() {
    localStorage.clear();
  }

  get isLoggedIn() {
    if (localStorage.getItem("user")) {
      return true;
    }
    return false;
  }
}

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private _router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot ): boolean {
    if (this.isLoggedIn)
      return true;
    this._router.navigate(["/admin-login"]);
  }

  get isLoggedIn() {
    if (localStorage.getItem("user")) {
      return true;
    }
    return false;
  }
}