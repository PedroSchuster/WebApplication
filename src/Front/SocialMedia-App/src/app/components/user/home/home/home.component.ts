import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { Post } from "@app/models/identity/Post";
import { PostTL } from "@app/models/identity/PostTl";
import { User } from "@app/models/identity/User";
import { AccountService } from "@app/services/account.service";
import { PostService } from "@app/services/post.service";
import { NgxSpinnerService } from "ngx-spinner";
import { Subscription } from "rxjs";


@Component({
  selector: 'home-root',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  post = {} as Post;
  subscription: Subscription;
  public posts: PostTL[] = [];

  constructor(public accountService: AccountService,
    private postService: PostService,
    private spinner: NgxSpinnerService,
    private router: Router) {}

  ngOnInit(): void {
  }


}
