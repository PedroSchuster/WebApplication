import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Message } from '@app/models/identity/Message';
import { UserConnection } from '@app/models/identity/UserConnection';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { SignalrService } from '@app/services/signalr.service';
import { error } from 'jquery';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-chat-details',
  templateUrl: './chat-details.component.html',
  styleUrls: ['./chat-details.component.scss']
})
export class ChatDetailsComponent implements OnInit {
  public messages = [] as Message[];
  msg = {} as Message;
  connectionId: number;
  loggedUserId: number;
  isChatOpen: boolean = true;
  @Input() selectedUser: UserUpdate;
  @Input() selectedUserConnection: UserConnection;
  @Output() closeChatEvent = new EventEmitter<void>();


  constructor(
    private signalrService: SignalrService,
    private router: Router,
    private spinner: NgxSpinnerService
  ){}

  ngOnInit(): void {
    this.sendMsgLis();
    this.getConnection();
    console.log(this.selectedUser)
    console.log(this.selectedUserConnection)
  }



  sendMsgInv(): void {
    if (this.msg?.body.trim() === "" || this.msg == null) return;

    this.signalrService.hubConnection.invoke("sendMsg", this.selectedUserConnection.connId, this.msg)
    .catch(err => console.error(err));

  }

  private sendMsgLis(): void {
    this.signalrService.hubConnection.on("sendMsgResponse", (connId: string, msg: Message) => {
      this.messages.push(msg);
    });
  }

  public updateMsg(event: any): void{
    this.msg.body = event.target.innerText;
  }

  private getConnection(): void{
    this.spinner.show();
    this.loggedUserId= JSON.parse(localStorage.getItem('user'))['userId'];
    this.signalrService.GetConnection(this.loggedUserId, this.selectedUser.id).subscribe(
      (response: number) => {
        this.connectionId = response
        this.getMessages();

      },
      (error: any) => console.log(error)
    ).add(()=>this.spinner.hide())
  }

  public sendMessage(): void{
    this.msg.connectionId = this.connectionId;
    let date = new Date();
    this.msg.dateTime = date.toLocaleString();
    this.msg.userId = this.loggedUserId;

    if (this.selectedUserConnection != null){
      this.sendMsgInv();
    }
    this.signalrService.SendMessage(this.msg).subscribe(
      () => {
        this.messages.push(this.msg);
        document.getElementsByClassName("textarea")[0].innerHTML = "";
        this.msg = {} as Message;

      },
      (error: any) => console.log(error)
    )
  }

  private getMessages(): void{
    this.signalrService.GetMessages(this.connectionId).subscribe(
      (response: any) => {this.messages = response},
      (error: any) => console.log(error)
    )
  }

  public closeChat() {
    this.closeChatEvent.emit();
  }
}
