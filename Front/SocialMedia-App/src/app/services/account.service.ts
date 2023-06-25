import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, map, take } from 'rxjs';
import { User } from '../models/identity/User';
import { UserUpdate } from '../models/identity/UserUpdate';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  // Esse replaySubject so vai pegar as requisiçoes depois de subscrever, entao se vc criar um interceptor dps de ja ter um user logado, vai dar erro
  // pq la embaixo da next(user) tlgd entao o interceptor n pegou esse user, ja q se inscreveu dps, mas isso eh so pq tu fez o interceptor dps
  private currentUserSource = new ReplaySubject<User | null>(1); // atualiza um objeto com o buffer de 1, ent vai ter so 1 n
  public currentUser$ = this.currentUserSource.asObservable();
  // A propriedade currentUser$ é uma forma de expor um Observable para os consumidores externos,
  // através do qual eles podem se inscrever para receber as atualizações do usuário atual,
  // mas somente a própria classe pode emitir novos valores para esse Observable
  // O símbolo $ ao final do nome é uma convenção comum em muitos frameworks
  // e bibliotecas de programação reativa para indicar que a variável é um Observable.

  baseURL = 'https://localhost:7209/api/account/';

  constructor(private http: HttpClient) { }

  public login(model: any): Observable<void>{
    return this.http.post<User>(this.baseURL + 'login', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if (user){
          this.setCurrentUser(user);
        }
      })
    );
  }

  public getUser(): Observable<UserUpdate>{
    return this.http.get<UserUpdate>(this.baseURL + 'getUser').pipe(take(1));
  }

  public getUserByUserName(userName: string): Observable<UserUpdate>{
    return this.http.get<UserUpdate>(`${this.baseURL}getUserByUserName/${userName}`).pipe(take(1));
  }

  public getUsersByFilter(filter: string): Observable<UserUpdate[]>{
    const formData = new FormData();
    formData.append('filter', filter);
    return this.http.post<any>(`${this.baseURL}getusersbyfilter`, formData).pipe(take(1));
  }

  public getUserById(id: number): Observable<User>{
    return this.http.get<User>(`${this.baseURL}getUser/${id}`).pipe(take(1));
  }

  public checkUserName(userName: string): Observable<string>{
    return this.http.get<string>(`${this.baseURL}checkusername/${userName}`).pipe(take(1));
  }

  public followUser(userName: string): Observable<string>{
    const formData = new FormData();
    formData.append('userName', userName);
    return this.http.put<any>(`${this.baseURL}follow`, formData).pipe(take(1));
  }

  public unfollowUser(userName: string): Observable<string>{
    const formData = new FormData();
    formData.append('userName', userName);
    return this.http.put<any>(`${this.baseURL}unfollow`, formData).pipe(take(1));
  }

  public updateUser(model: UserUpdate): Observable<void>{
    return this.http.put<UserUpdate>(this.baseURL + 'updateuser', model).pipe(
      take(1),
      map((user: UserUpdate) => {
        this.setCurrentUser(user);
      })
    );
  }

  public uploadImage(file: File): Observable<UserUpdate>{
    const fileToUpload = file[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload);

    return this.http.post<UserUpdate>(`${this.baseURL}upload-image`, formData).pipe(take(1));
  }

  public register(model: any): Observable<void>{
    return this.http.post<User>(this.baseURL + 'register', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if (user){
          this.setCurrentUser(user);
        }
      })
      );
  }

  logout(): void{
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  // classes externas podem se increver (subscribe) currentUser$ e receber esses valores de next, complete q a gente setou aqui

  public setCurrentUser(user: User): void{
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

}
