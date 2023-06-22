import { HttpClient } from '@angular/common/http';
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
  private postsSource = new BehaviorSubject(this.timelinePosts);
  public currentPosts$ = this.postsSource.asObservable();

  updateTimeLine(){

  }

  constructor(private http: HttpClient) { }


  public getPosts(userId:number): Observable<PostTL[]>{
    return this.http.get<PostTL[]>(`${this.baseURL}/getposts/${userId}`).pipe(take(1));
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
