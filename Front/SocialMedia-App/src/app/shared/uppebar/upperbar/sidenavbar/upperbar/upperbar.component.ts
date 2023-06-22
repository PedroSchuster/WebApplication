import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';

@Component({
  selector: 'app-upperbar',
  templateUrl: './upperbar.component.html',
  styleUrls: ['./upperbar.component.scss']
})
export class UpperbarComponent {
  isCollapsed = true;
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


}
