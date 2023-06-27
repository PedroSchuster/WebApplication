import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Message } from '@app/models/identity/Message';
import { UserConnection } from '@app/models/identity/UserConnection';
import { Localhost } from '@app/util/localhost';
import * as signalR from '@aspnet/signalr';
import { ToastrService } from 'ngx-toastr';
import { Subject, Observable, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  constructor(
    public toastr: ToastrService,
    public router: Router,
    private http: HttpClient
    ) { }

  hubConnection: signalR.HubConnection;
  baseURL = Localhost.LOCALHOST + 'api/chat/';
  userData: UserConnection;

  ssSubj = new Subject<any>();
  ssObs(): Observable<any> {
      return this.ssSubj.asObservable();
  }

  usersList: UserConnection[] = [];

  connectedUsersSubj = new Subject<UserConnection[]>();
  connectedUsers(): Observable<UserConnection[]> {
      return this.connectedUsersSubj.asObservable();
  }

  public addConnectedUser(user: UserConnection): void{
    this.usersList.push(user);
    this.connectedUsersSubj.next(this.usersList);
  }

  startConnection = () => {
      this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(Localhost.LOCALHOST + 'chat', {
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets
      })
      .build();

      this.hubConnection
      .start()
      .then(() => {
        if (this.hubConnection.state == 1)
          this.ssSubj.next(true);
      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public GetMessages(connId: number): Observable<any>{
    return this.http.get<Message[]>(`${this.baseURL}messages/${connId}`).pipe(take(1));
  }

  public GetConnection(userId: number, targetId: number): Observable<any>{
    return this.http.get<any>(`${this.baseURL}connection/${userId}/${targetId}`).pipe(take(1));
  }

  public SendMessage(message: Message): Observable<any>{
    return this.http.post<any>(`${this.baseURL}send-message`, message).pipe(take(1));
  }
}
