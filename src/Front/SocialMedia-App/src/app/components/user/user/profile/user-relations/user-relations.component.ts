import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-user-relations',
  templateUrl: './user-relations.component.html',
  styleUrls: ['./user-relations.component.scss']
})
export class UserRelationsComponent implements OnInit {
  private userName: string;
  public user: UserUpdate;

  public listSource: UserUpdate[] = [];
  public type: string;

  constructor(private activeRouter: ActivatedRoute,
              private router: Router,
              private spinner: NgxSpinnerService,
              private accountService: AccountService) { }

  ngOnInit() {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.getType();
    this.loadProfile();
  }

  private loadProfile(): void{
    this.userName = this.activeRouter.snapshot.paramMap.get('userName');
    this.spinner.show();
    if (this.userName != null && this.userName != ''){
      this.accountService.getUserByUserName(this.userName).subscribe(
        (response: UserUpdate) => {
          this.user = response;
          if (this.type === 'followers'){
            this.listSource = this.user.followers;
          } else{
            this.listSource = this.user.following;
          }
        },
        (error: any) => console.error(error)
      ).add(()=>this.spinner.hide());
    }
  }

  public getType():any{
    this.type = this.activeRouter.snapshot.paramMap.get('type');
  }
}
