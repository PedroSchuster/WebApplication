import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(public toaster: ToastrService, public router: Router) {}

  canActivate(): boolean{
    if (localStorage.getItem('user') !== null)
      return true;
    this.toaster.info("Usuário não autenticado")
    this.router.navigate(['user/login']);
    return false;
  }
}
