import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Post } from '@app/models/identity/Post';
import { PostDetails } from '@app/models/identity/PostDetails';
import { PostTL } from '@app/models/identity/PostTl';
import { BehaviorSubject, Observable, ReplaySubject, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class PostService {

  baseURL = 'https://localhost:7209/api/post';

  public timelinePosts: PostTL[] = [];
  postsSource = new BehaviorSubject<PostTL[]>([]);
  public currentPosts$ = this.postsSource.asObservable();

  postsSourceProfile = new BehaviorSubject<PostTL[]>([]);
  public currentPostsProfile$ = this.postsSourceProfile.asObservable();

  public changeTLType(type: string): void{
    localStorage.setItem('tlType', type);
  }

  public updateTimeLine(userId:number, pageNumber?: number, itemsPerPage?: number){
    const tlType = localStorage.getItem('tlType');
    console.log(tlType)
    console.log(pageNumber)
    console.log(itemsPerPage)
    if (tlType === 'following'){
      this.getPostsFollowingPage(userId, pageNumber, itemsPerPage).subscribe(
        (response: PostTL[]) => this.postsSource.next(response),
        (error: any) => console.error(error)
      )
    }
    else if (tlType === 'home'){
      this.getPostsHomePage(userId, pageNumber, itemsPerPage).subscribe(
        (response: PostTL[]) => {this.postsSource.next(response); console.log(response)},
        (error: any) => console.error(error)
      )
    }
    else if (tlType === 'profile'){
      this.getPosts(userId, pageNumber, itemsPerPage).subscribe(
        (response: PostTL[]) => {this.postsSourceProfile.next(response); console.log(response)},
        (error: any) => console.error(error)
      )
    }

  }

  constructor(private http: HttpClient) {

  }


  public getPosts(userId:number, pageNumber, itemsPerPage): Observable<PostTL[]>{
    const pageParams = {
      PageNumber: pageNumber,
      PageSize: itemsPerPage
    };

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    return this.http.post<PostTL[]>(`${this.baseURL}/getposts/${userId}`, pageParams, httpOptions ).pipe(take(1));
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

}
