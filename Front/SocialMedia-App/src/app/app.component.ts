import { Component } from '@angular/core';
import { User } from './models/identity/User';
import { AccountService } from './services/account.service';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Post } from './models/identity/Post';
import { PostService } from './services/post.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  isLoading = false;
  currentPage=1;
  itensPerPage=10;

  public posts: Post[] = [];

  toggleLoading = () => this.isLoading = !this.isLoading;

  constructor(public accountService: AccountService,
    private postService: PostService,
    private spinner: NgxSpinnerService,
    private router: Router) {}

  ngOnInit(): void {
    this.setCurrentUser();
    this.spinner.show();
    this.carregarPosts();
  }

  setCurrentUser(): void{
    let user: User | null;
    if (localStorage.getItem('user')){
      user = JSON.parse(localStorage.getItem('user') ?? '{}');
    } else {
      user = null;
    }
    // qndo iniciara a aplicaçao vai pegar o user do localstorage e setar como currentUser pra caso recarregar a pagina n perca o user
    if (user !== null)
      this.accountService.setCurrentUser(user);
  }


  public appendData():void {
    this.toggleLoading();
    this.postService.getPosts().subscribe(
      (response: Post[]) => this.posts = [...this.posts, ...response],
      (error: any) => console.log("Ocorreu um erro em carregar os posts" + error)
    ).add(()=> {
      this.spinner.hide();
      this.toggleLoading();
    });
  }

  public carregarPosts(): void{
    this.toggleLoading();
    this.postService.getPosts().subscribe(
      (response: Post[]) => {this.posts = response; console.log(response)},
      (error: any) => console.log("Ocorreu um erro em carregar os posts" + error)
    ).add(()=> {
      this.spinner.hide();
      this.toggleLoading();
    });
  }

  public Teste():void{
    console.log("dasdsadas");

  }

  public onScroll(): void{
    console.log("aaaa")

    this.currentPage++;
    this.appendData();
  }
}
