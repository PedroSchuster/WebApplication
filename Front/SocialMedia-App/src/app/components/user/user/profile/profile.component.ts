import { Component, OnInit } from '@angular/core';
import { Post } from '@app/models/identity/Post';
import { PostTL } from '@app/models/identity/PostTl';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { PostService } from '@app/services/post.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  public posts: PostTL[] = [];
  public user = {} as UserUpdate;
  public editMode = false;
  constructor(private postService: PostService,
    private spinner: NgxSpinnerService,
    private accountService: AccountService){
  }

  ngOnInit(): void {
    this.loadProfile();
    this.loadPosts();
  }

  private loadPosts(): void{
    this.spinner.show();
    this.postService.getPosts().subscribe(
      (response: PostTL[]) => this.posts = response,
      (error: any) => console.error(error)
    ).add(()=>this.spinner.hide());
  }

  private loadProfile(): void{
    this.spinner.show();
    this.accountService.getUser().subscribe(
      (response: UserUpdate) => this.user = response,
      (error: any) => console.error(error)
    ).add(()=>this.spinner.hide());
  }
}
