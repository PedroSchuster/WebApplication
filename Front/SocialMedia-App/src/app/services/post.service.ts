import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Post } from '@app/models/identity/Post';
import { Observable, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  baseURL = 'https://localhost:7209/api/post/';

  constructor(private http: HttpClient) { }

  public getPosts(): Observable<Post[]>{
    return this.http.get<Post[]>(this.baseURL).pipe(take(1));
  }

  public getPostById(id: number): Observable<Post>{
    return this.http.get<Post>(`${this.baseURL}/${id}`).pipe(take(1));
  }

  public getCommentsByPostId(id: number): Observable<Post[]>{
    return this.http.get<Post[]>(`${this.baseURL}/comments/${id}`).pipe(take(1));
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
