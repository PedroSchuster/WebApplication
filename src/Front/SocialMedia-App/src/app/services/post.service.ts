import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Post } from '@app/models/identity/Post';
import { PostDetails } from '@app/models/identity/PostDetails';
import { PostTL } from '@app/models/identity/PostTl';
import { Localhost } from '@app/util/localhost';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject, Observable, ReplaySubject, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class PostService {

  baseURL = Localhost.LOCALHOST + 'api/post';

  public timelinePosts: PostTL[] = [];
  postsSource = new BehaviorSubject<PostTL[]>([]);
  public currentPosts$ = this.postsSource.asObservable();

  postsSourceProfile = new BehaviorSubject<PostTL[]>([]);
  public currentPostsProfile$ = this.postsSourceProfile.asObservable();

  constructor(private http: HttpClient,
    private spinner: NgxSpinnerService) {
  }

  public changeProfileUserName(userName: string): void{
    localStorage.setItem('profileUserName', userName);
  }

  public changeTLType(type: string): void{
    localStorage.setItem('tlType', type);
  }

  public setPageNumber(pageNumber: string): void{
    localStorage.setItem('pageNumber', pageNumber);
  }

  public updateTimeLine(userId:number, itemsPerPage?: number){
    const tlType = localStorage.getItem('tlType');
    if (tlType === 'post-detail') return;

    this.spinner.show();

    const pageNumber = parseInt(localStorage.getItem('pageNumber'));

    if (tlType === 'following'){
      this.getPostsFollowingPage(userId, pageNumber, itemsPerPage).subscribe(
        (response: PostTL[]) => this.postsSource.next(response),
        (error: any) => console.error(error)
      ).add(()=>this.spinner.hide());
    }
    else if (tlType === 'home'){
      this.getPostsHomePage(userId, pageNumber, itemsPerPage).subscribe(
        (response: PostTL[]) => this.postsSource.next(response),
        (error: any) => console.error(error)
      ).add(()=>this.spinner.hide());
    }
    else if (tlType === 'profile'){
      let userName = localStorage.getItem('profileUserName');
      this.getPosts(userName, pageNumber, itemsPerPage).subscribe(
        (response: PostTL[]) => this.postsSourceProfile.next(response),
        (error: any) => console.error(error)
      ).add(()=>this.spinner.hide());
    }

  }

  public getPosts(username:string, pageNumber, itemsPerPage): Observable<PostTL[]>{
    const pageParams = {
      PageNumber: pageNumber,
      PageSize: itemsPerPage
    };

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    return this.http.post<PostTL[]>(`${this.baseURL}/getposts/${username}`, pageParams, httpOptions ).pipe(take(1));
  }

  public getPostsFollowingPage(userId:number, pageNumber?: number, itemsPerPage?: number): Observable<PostTL[]>{
    const pageParams = {
      PageNumber: pageNumber,
      PageSize: itemsPerPage
    };

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };
    return this.http.post<PostTL[]>(`${this.baseURL}/getposts/following/${userId}`, pageParams, httpOptions).pipe(take(1));
  }

  public getPostsHomePage(userId: number, pageNumber?: number, itemsPerPage?: number): Observable<PostTL[]> {
    const pageParams = {
      PageNumber: pageNumber,
      PageSize: itemsPerPage
    };

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    return this.http.post<PostTL[]>(`${this.baseURL}/getposts/home/${userId}`, pageParams, httpOptions).pipe(take(1));
  }

  public getPostById(id: number): Observable<PostDetails>{
    return this.http.get<PostDetails>(`${this.baseURL}/getpostbyid/${id}`).pipe(take(1));
  }

  public getCommentsByPostId(id: number): Observable<PostTL[]>{
    return this.http.get<PostTL[]>(`${this.baseURL}/comments/${id}`).pipe(take(1));
  }

  public addPost(post: Post): Observable<Post>{
    return this.http.post<Post>(this.baseURL, post).pipe(take(1));
  }

  public updatePost(id: number, post: Post): Observable<Post>{
    return this.http.put<Post>(`${this.baseURL}/${id}`, post).pipe(take(1));
  }

  public deletePost(id: number): Observable<Post>{
    return this.http.delete<Post>(`${this.baseURL}/${id}`).pipe(take(1));
  }

  public like(postId: number): Observable<any>{
    return this.http.get<any>(`${this.baseURL}/like/${postId}`).pipe(take(1));
  }

  public removelike(postId: number): Observable<any>{
    return this.http.get<any>(`${this.baseURL}/removelike/${postId}`).pipe(take(1));
  }

}
