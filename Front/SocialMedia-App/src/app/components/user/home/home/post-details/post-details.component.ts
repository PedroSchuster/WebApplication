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
  comments: PostTL[] = [];
  comment = {} as Post;
  parensPostTL: PostTL[] = [];

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
      },
      (error: any) => console.error(error)
    ).add(()=>this.spinner.hide());
  }


  public addComment(): void{
    let date = new Date();
    this.comment.date =  date.toLocaleDateString();
    this.comment.rootId = this.postId;
    this.postService.addPost(this.comment).subscribe(
      () => {
        this.comment = {} as Post;
        document.getElementsByClassName("textarea")[0].innerHTML = "";
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
  redirectTo(uri: string) {
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() =>
    this.router.navigate([uri]));
 }
}
