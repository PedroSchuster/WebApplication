import { Component, Input, OnInit } from '@angular/core';
import { Params, Router } from '@angular/router';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { Localhost } from '@app/util/localhost';

@Component({
  selector: 'app-profile-card',
  templateUrl: './profile-card.component.html',
  styleUrls: ['./profile-card.component.scss']
})
export class ProfileCardComponent implements OnInit {

  public user: UserUpdate;
  public profilePicURL: string;

  @Input() userName;
  @Input() isSearch = false;
  constructor(private accountService: AccountService,
              private router: Router) { }

  ngOnInit() {
    this.loadUser();
  }

  private loadUser(){
    this.profilePicURL = 'assets/images/empty-profile.png';
    this.accountService.getUserByUserName(this.userName).subscribe(
      (response: UserUpdate) => {
        this.user = response
        if (response.profilePicURL != null && response.profilePicURL != '' && response.profilePicURL != undefined)
          this.profilePicURL = Localhost.LOCALHOST + 'Resources/Images/' + response.profilePicURL;
      },
      (error: any) => console.log(error)
    )
  }

  public userDetails(): void{
    console.log(this.userName)
    this.navigateToSameRouteWithDifferentParams(this.userName)
  }

  navigateToSameRouteWithDifferentParams(newParams: Params) {
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      // Navega para uma rota diferente temporariamente
      this.router.navigateByUrl('user/profile/' + newParams);
    });
  }

}
