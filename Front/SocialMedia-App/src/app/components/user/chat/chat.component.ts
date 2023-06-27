import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Message } from '@app/models/identity/Message';
import { UserConnection } from '@app/models/identity/UserConnection';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { SignalrService } from '@app/services/signalr.service';
import { NgxSpinnerService, Spinner } from 'ngx-spinner';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {

  constructor(
    private signalrService: SignalrService,
    private router: Router,
    private spinner: NgxSpinnerService,
    private accountService: AccountService
  ){}

  isChatOpen: boolean = false;
  users: UserConnection[] = [];
  following: UserUpdate[];
  selectedUserConnection = {} as UserConnection;
  selectedUser: UserUpdate;

  ngOnInit(): void {
    this.users = this.signalrService.usersList;
    this.loadProfile();
  }

  private loadProfile(): void{
    let loggedUserName = JSON.parse(localStorage.getItem('user'))['userName'];
      this.spinner.show();
      this.accountService.getUserByUserName(loggedUserName).subscribe(
        (response: UserUpdate) => {
          this.following = response.following;
        },
        (error: any) => console.error(error)
      ).add(()=>this.spinner.hide());
  }

  public openChat(user: UserUpdate){
    this.selectedUser = user;
    this.selectedUserConnection = this.users.filter(x=>x.userId == user.id).length > 0?
                                  this.users.filter(x=>x.userId == user.id)[0] : null;
    this.isChatOpen = true;
  }

  onCloseChat() {
    this.isChatOpen = false; // Define como false para fechar o componente
  }
}
