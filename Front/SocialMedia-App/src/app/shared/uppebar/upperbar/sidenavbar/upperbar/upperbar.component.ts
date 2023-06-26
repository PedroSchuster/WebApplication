import { Component } from '@angular/core';
import { Params, Router } from '@angular/router';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';

@Component({
  selector: 'app-upperbar',
  templateUrl: './upperbar.component.html',
  styleUrls: ['./upperbar.component.scss']
})
export class UpperbarComponent {
  isCollapsed = true;
  imgURL: string;
  constructor(public accountService: AccountService,
    private router: Router) {}

  ngOnInit(): void {

  }

  logout(): void{
    this.accountService.logout();
    this.router.navigateByUrl('/user/login');
  }

  showMenu(): boolean{
      return this.router.url != '/user/login';
    }

    public userDetails(userName: any): void{
      this.navigateToSameRouteWithDifferentParams(userName)
    }

    navigateToSameRouteWithDifferentParams(newParams: Params) {
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
        // Navega para uma rota diferente temporariamente
        this.router.navigateByUrl('user/profile/' + newParams);
      });
    }
}
