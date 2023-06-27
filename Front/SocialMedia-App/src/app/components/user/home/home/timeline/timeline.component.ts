import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Post } from '@app/models/identity/Post';
import { PostTL } from '@app/models/identity/PostTl';
import { User } from '@app/models/identity/User';
import { AccountService } from '@app/services/account.service';
import { PostService } from '@app/services/post.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subscription, take } from 'rxjs';

@Component({
  selector: 'app-timeline',
  templateUrl: './timeline.component.html',
  styleUrls: ['./timeline.component.scss']
})
export class TimelineComponent implements OnInit{

  post = {} as Post;
  subscription: Subscription;
  public posts: PostTL[] = [];

  private pageNumber?: number = 1;
  private itemsPerPage?: number = 10;
  private userId: number;
  public currentTLType: string = '';

  public profilePicURL: string;

  constructor(private accountService: AccountService,
    private postService: PostService,
    private spinner: NgxSpinnerService,
    private router: Router) {}

  ngOnInit(): void {
    this.userId = JSON.parse(localStorage.getItem('user'))['userId'];
    this.loadCurrentUser();
    this.selectHomePage();
    this.subscription = this.postService.currentPosts$.subscribe(response => this.posts.push(...response))
    this.posts = [];
  }

  private loadCurrentUser(): void{
    this.accountService.currentUser$.subscribe(
      (response: User) => {
        if (response?.profilePicURL != null && response?.profilePicURL != ''){
          this.profilePicURL = 'https://localhost:7209/Resources/Images/' + response?.profilePicURL;
        } else{
          this.profilePicURL = 'assets/images/empty-profile.png';
        }
      },
      (error: any) => console.error(error)
      )
  }

  public addPost(): void{
    let date = new Date();
    this.post.date = date.toLocaleString();
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
    this.postService.setPageNumber( this.pageNumber.toString())
    this.postService.updateTimeLine(this.userId, this.itemsPerPage);
  }

  public selectHomePage(): void{

    const element = document.getElementById('main');
    if (element) {
      element.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }

    this.postService.changeTLType('home');

    this.postService.setPageNumber('1');

    this.updateTimeLine();


  }

  public selectFollowingPage(): void{

    const element = document.getElementById('main');
    if (element) {
      element.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }

    this.postService.changeTLType('following');

    this.postService.setPageNumber('1');

    this.updateTimeLine();

  }
}
