import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserLogin } from 'src/app/models/identity/UserLogin';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  model = {} as UserLogin;

  constructor(private accountService: AccountService,
              private router: Router,
              private toaster: ToastrService) {}

  ngOnInit(): void {}

  public login(): void{
    this.accountService.login(this.model).subscribe(
      () => this.router.navigateByUrl('/user/registration'),
      (error: any) => {
        if (error.status == 401)
          this.toaster.error('Usuário ou senha inválido');
        else
          console.error(error);
      }
    )
  }
}
