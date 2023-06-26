import { ChangeDetectorRef, Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { Post } from '@app/models/identity/Post';
import { PostTL } from '@app/models/identity/PostTl';
import { User } from '@app/models/identity/User';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { PostService } from '@app/services/post.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-post-tl',
  templateUrl: './post-tl.component.html',
  styleUrls: ['./post-tl.component.scss']
})
export class PostTLComponent implements OnInit  {

  @Input() post: PostTL;
  totalComments:number;
  public imgURL: string;
  public isLiked: boolean = false;
  public totalLikes: number;

  constructor(
    private router: Router,
    private postService: PostService
  ) { }

  public ngOnInit(): void {
    if (this.post.user?.profilePicURL != null && this.post.user?.profilePicURL != ''){
      this.imgURL = 'https://localhost:7209/Resources/Images/' + this.post.user.profilePicURL;
    } else{
      this.imgURL = 'assets/images/empty-profile.png';
    }

    this.isLiked = this.post.isLiked;
    this.totalLikes = this.post.totalLikes;
  }

  public postDetails(): void{
    console.log(this.post)
    this.router.navigate([`home/post/${this.post.id}`])
  }

  public like(event: any): void{
    event.stopPropagation();
    if (!this.isLiked){
      this.postService.like(this.post.id).subscribe(
        () => {
          this.isLiked = true
          this.totalLikes ++;
        },
        (error: any) => console.error(error)
      )
    } else{
      this.postService.removelike(this.post.id).subscribe(
        () => {
          this.isLiked = false
          this.totalLikes --;
        },
        (error: any) => console.error(error)
      )
    }
  }

}
