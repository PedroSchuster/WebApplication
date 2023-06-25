import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';

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
          this.profilePicURL = 'https://localhost:7209/Resources/Images/' + response.profilePicURL;
      },
      (error: any) => console.log(error)
    )
  }

  public userDetails(): void{
    this.router.navigateByUrl('user/profile/' + this.userName)
  }
}
