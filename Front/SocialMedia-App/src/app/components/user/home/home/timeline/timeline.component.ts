import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Post } from '@app/models/identity/Post';
import { PostTL } from '@app/models/identity/PostTl';
import { AccountService } from '@app/services/account.service';
import { PostService } from '@app/services/post.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-timeline',
  templateUrl: './timeline.component.html',
  styleUrls: ['./timeline.component.scss']
})
export class TimelineComponent implements OnInit {

  post = {} as Post;
  subscription: Subscription;
  public posts: PostTL[] = [];

  private pageNumber?: number = 1;
  private itemsPerPage?: number = 10;
  private userId: number;
  public currentTLType: string = '';

  constructor(private accountService: AccountService,
    private postService: PostService,
    private spinner: NgxSpinnerService,
    private router: Router) {}

  ngOnInit(): void {
    this.userId = JSON.parse(localStorage.getItem('user'))['userId'];
    this.selectHomePage();
    this.subscription = this.postService.currentPosts$.subscribe(response => this.posts.push(...response))
  }

  public addPost(): void{
    let date = new Date();
    this.post.date =  date.toLocaleDateString();
    this.postService.addPost(this.post).subscribe(
      () => {
        this.post = {} as Post;
        document.getElementsByClassName("textarea")[0].innerHTML = "";
        this.posts = [];
        this.updateTimeLine();
      },
      (error: any) => console.error(error)
    );
  }

  public updatePost(event: any): void{
    this.post.body = event.target.innerText;
  }

  private updateTimeLine(): void{
    this.posts = [];
    this.postService.updateTimeLine(this.userId, this.pageNumber, this.itemsPerPage);
  }

  public selectHomePage(): void{
    this.postService.changeTLType('home');
    this.currentTLType = localStorage.getItem('tlType');
    this.updateTimeLine();
  }

  public selectFollowingPage(): void{
    this.postService.changeTLType('following');
    this.currentTLType = localStorage.getItem('tlType');
    this.updateTimeLine();
  }
}
