import { Component } from '@angular/core';
import { User } from './models/identity/User';
import { AccountService } from './services/account.service';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Post } from './models/identity/Post';
import { PostService } from './services/post.service';
import { Subscription, take, timeout } from 'rxjs';
import { SignalrService } from './services/signalr.service';
import { UserConnection } from './models/identity/UserConnection';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  private pageNumber?: number = 1;
  private itemsPerPage?: number = 10;
  private userId: number;
  private currentRoute: string;
  public isNotProfileRoute: boolean;

  private userConnection: UserConnection;
  private isConnected: boolean;

  constructor(public accountService: AccountService,
    private postService: PostService,
    private spinner: NgxSpinnerService,
    private signalrService: SignalrService,
    private router: Router) {}

  ngOnInit(): void {
    this.router.events.subscribe((event: any) => {
      if (event instanceof NavigationEnd) {
          this.currentRoute = (<NavigationEnd>event).url;
          this.isNotProfileRoute = this.currentRoute.indexOf('profile') === -1;
      }
    });

    this.setConnection();
    this.setCurrentUser();
    this.userId = JSON.parse(localStorage.getItem('user'))['userId'];
  }

  private setConnection(): void{
    this.signalrService.startConnection();
  this.signalrService.ssObs().pipe(take(1)).subscribe(()=>{
    if (this.signalrService.hubConnection.state == 1){
      this.signalrService.hubConnection.invoke("GetUserConnectionAsync").catch(err => console.error(err))

      this.signalrService.hubConnection.on('getUserConnectionResponse', (connectionId: string) => {
      let currentUserName = JSON.parse(localStorage.getItem('user'))['userName'];
      let currentUserId= JSON.parse(localStorage.getItem('user'))['userId'];
      this.userConnection = {userName: currentUserName, userId: currentUserId, connId: connectionId}
      this.signalrService.addConnectedUser(this.userConnection)
    })
    }
  })

  }

  public onScroll(): void{
    if (localStorage.getItem('pageNumber') == '1'){
      this.pageNumber = parseInt(localStorage.getItem('pageNumber'))
    }
    this.pageNumber ++;

    this.postService.setPageNumber(this.pageNumber.toString());

    this.postService.updateTimeLine(this.userId, this.itemsPerPage);

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
