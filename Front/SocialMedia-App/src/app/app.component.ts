import { Component } from '@angular/core';
import { User } from './models/identity/User';
import { AccountService } from './services/account.service';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Post } from './models/identity/Post';
import { PostService } from './services/post.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  private pageNumber?: number = 2;
  private itemsPerPage?: number = 10;
  private userId: number;
  private type: string;

  constructor(public accountService: AccountService,
    private postService: PostService,
    private spinner: NgxSpinnerService,
    private router: Router) {}

  ngOnInit(): void {
    this.setCurrentUser();
    this.userId = JSON.parse(localStorage.getItem('user'))['userId'];

  }

  public onScroll(): void{
    const currentTlType = localStorage.getItem('tlType');
    this.pageNumber ++;
    if (this.type != currentTlType){
      this.type = currentTlType;
      this.pageNumber = 2;
    }
    this.postService.updateTimeLine(this.userId, this.pageNumber, this.itemsPerPage);
  }

  setCurrentUser(): void{
    let user: User | null;
    if (localStorage.getItem('user')){
      user = JSON.parse(localStorage.getItem('user') ?? '{}');
    } else {
      user = null;
    }
    // qndo iniciara a aplicaçao vai pegar o user do localstorage e setar como currentUser pra caso recarregar a pagina n perca o user
    if (user !== null)
      this.accountService.setCurrentUser(user);
    }
}
