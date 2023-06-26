import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Post } from '@app/models/identity/Post';
import { PostDetails } from '@app/models/identity/PostDetails';
import { PostTL } from '@app/models/identity/PostTl';
import { PostService } from '@app/services/post.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-post-details',
  templateUrl: './post-details.component.html',
  styleUrls: ['./post-details.component.scss']
})
export class PostDetailsComponent implements OnInit {
  post = {} as PostDetails;
  postId: number;
  userId: number;
  comments: PostTL[] = [];
  comment = {} as PostDetails;
  parensPostTL: PostTL[] = [];
  imgURL: string;

  public isLiked: boolean = false;
  public totalLikes: number;

  constructor(private activedRoute: ActivatedRoute,
              private router: Router,
              private postService: PostService,
              private spinner: NgxSpinnerService) {}

  public ngOnInit(): void {
    this.loadPost();
  }

  private loadPost():void{
    this.spinner.show();
    this.postId = +this.activedRoute.snapshot.paramMap.get('id');
    this.postService.getPostById(this.postId).subscribe(
      (response: PostDetails) => {
        this.post = response
        console.log(this.post)
        this.comments = this.post.comments
        this.isLiked = this.post.isLiked;
        this.totalLikes = this.post.totalLikes;
        this.postService.changeTLType('post-detail');
        if (this.post.user?.profilePicURL != null && this.post.user?.profilePicURL != ''){
          this.imgURL = 'https://localhost:7209/Resources/Images/' + this.post.user.profilePicURL;
        } else{
          this.imgURL = 'assets/images/empty-profile.png';
        }

      },
      (error: any) => console.error(error)
    ).add(()=>this.spinner.hide());
  }

  public addComment(): void{
    let date = new Date();
    this.comment.date =  date.toLocaleString();
    this.comment.rootId = this.postId;
    this.post.totalComments += 1;
    this.postService.addPost(this.comment).subscribe(
      () => {
        this.comment = {} as PostDetails;
        document.getElementsByClassName("textarea")[0].innerHTML = "";
        this.postService.updatePost(this.postId, this.post).subscribe();
        this.loadPost();
      },
      (error: any) => console.error(error)
    );
  }

  public updatePost(event: any){
    this.comment.body = event.target.innerText;
  }

  public postDetails(id: number): void{
    this.redirectTo(`home/post/${id}`);
  }

  public redirectTo(uri: string) {
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() =>
    this.router.navigate([uri]));
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
