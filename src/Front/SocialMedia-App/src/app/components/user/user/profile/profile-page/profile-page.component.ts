import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, AbstractControlOptions, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { PostTL } from '@app/models/identity/PostTl';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { PostService } from '@app/services/post.service';
import { Localhost } from '@app/util/localhost';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Subject, Subscription, debounceTime } from 'rxjs';

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.scss']
})
export class ProfilePageComponent implements OnInit {

  form!: FormGroup;

  public charCount: number = 0;
  public maxChar: number = 128;

  public userName: string;
  public loggedUserName: string;
  public posts: PostTL[] = [];
  public user = {} as UserUpdate;
  public listParameter: UserUpdate[] = [];
  public editMode = false;
  public followMode = true;
  public isPopupOpen = false;


  userNameChanged: Subject<string> = new Subject<string>();
  profilePicURL = '';
  file: File;

  get f(): any { return this.form.controls; }


  subscription: Subscription;
  private pageNumber?: number = 1;
  private itemsPerPage?: number = 10;

  constructor(private postService: PostService,
    private spinner: NgxSpinnerService,
    private accountService: AccountService,
    private fb: FormBuilder,
    private activeRouter: ActivatedRoute,
    private router: Router,
    private dialogRef: MatDialog,
    private toaster: ToastrService){
  }

  ngOnInit(): void {
    console.log('aaaa')
    this.subscription = this.postService.currentPostsProfile$.subscribe(response => this.posts.push(...response),
    (error: any) => console.log(error));
    this.loadProfile();
    this.validation();

  }

  private loadPosts(): void{
    this.postService.changeTLType('profile');
    this.updateTimeLine();
  }

  private updateTimeLine(): void{
    this.posts = [];
    this.postService.setPageNumber( this.pageNumber.toString())
    this.postService.changeProfileUserName(this.user.userName)
    this.postService.updateTimeLine(this.user.id, this.itemsPerPage);
  }

  private loadProfile(): void{
    this.userName = this.activeRouter.snapshot.paramMap.get('userName');
    console.log(this.userName)
    this.loggedUserName = JSON.parse(localStorage.getItem('user'))['userName'];
    this.profilePicURL = 'assets/images/empty-profile.png';
    if (this.userName != null && this.userName != ''){
      this.spinner.show();
      this.accountService.getUserByUserName(this.userName).subscribe(
        (response: UserUpdate) => {
          this.user = response;
          console.log(response)
          this.charCount = this.user.bio != null? this.user.bio.length : 0;
          this.loadPosts();
          if (response.followers.some(x=>x.userName == this.loggedUserName)){
            this.followMode = false;
          } else{
            this.followMode = true;
          }
          if (response.profilePicURL != null && response.profilePicURL != '' && response.profilePicURL != undefined)
            this.profilePicURL = Localhost.LOCALHOST + 'Resources/Images/' + response.profilePicURL;

          this.form.patchValue(this.user);
        },
        (error: any) => console.error(error)
      ).add(()=>this.spinner.hide());
    }
  }
  private validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmPassword')
    };
    this.form = this.fb.group({
      id: [''],
      userName: ['', [Validators.required, Validators.maxLength(10)]],
      firstName: ['', [Validators.required, Validators.maxLength(10)]],
      lastName: ['', [Validators.required, Validators.maxLength(15)]],
      password: ['', [Validators.minLength(6), Validators.maxLength(30)]],
      email: ['', [Validators.email]],
      phoneNumber: ['', Validators.pattern('[- +()0-9]+')],
      bio: ['', Validators.maxLength(this.maxChar)],
      confirmPassword: ['', ]
    }, formOptions);
  }

  public valueChange(event:any):void{
    this.charCount = event? event.length : 0;
  }
  public checkUserName(event: any): void {
    if (event == null || event == undefined || event == '') return;
    if (this.userNameChanged.observers.length === 0) {
      this.userNameChanged.pipe(debounceTime(500)).subscribe(
        (value: string) => {
          this.accountService.checkUserName(value).subscribe(
            () => {
              const errors = this.f['userName'].errors;
              if (errors) {
                delete errors.invalidUserName;
                this.f['userName'].setErrors(Object.keys(errors).length > 0 ? errors : null);
              }
            },
            () => {
              const errors = this.f['userName'].errors || {};
              errors.invalidUserName = true;
              this.f['userName'].setErrors(errors);
            }
          );
        }
      );
    }
    this.userNameChanged.next(event);
  }


  public onFileChange(ev: any): void{
    const reader = new FileReader();

    reader.onload = (event: any) => this.profilePicURL = event.target.result;

    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]); // quando chama esse metodo, vai dar sobescrever o onLoad, e como onLoad recebe essa arrow function, vai mudar o profilePicURL
    // dai no caso o event passado como parametro no onLoad, vai ser this.file[0]

    this.uploadImagem();

  }

  private uploadImagem(): void{
    this.spinner.show();
    this.accountService.uploadImage(this.file).subscribe(
      () => {
        this.loadProfile(); // recarregar pagina pois a gente deu update la no back
      },
      (error: any) => {
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }

  public saveProfile(): void {
    this.spinner.show();
    if (this.form.valid){
      this.accountService.updateUser(this.form.value).subscribe(
        () => {
          this.toaster.success("Usuário atualizado com sucesso!", "Sucesso");
          this.toggleEditMode();
          this.loggedUserName = JSON.parse(localStorage.getItem('user'))['userName'];
          if (this.loggedUserName != this.userName){
            this.router.navigateByUrl('/user/profile/' + this.loggedUserName).then(() => {
              this.loadProfile();
            });
          } else {
            this.loadProfile();
          }
        },
        (error: any) => {
          console.log(error);
          this.toaster.error("Ocorreu um erro em atualizar o usuário!", "Erro")
        }
      ).add(() => this.spinner.hide());
    }
  }

  logout(): void{
    this.accountService.logout();
    this.router.navigateByUrl('/user/login');
  }

  public followUser(): void{
    if (this.userName === null || this.userName === '') return;
    this.spinner.show();
    this.accountService.followUser(this.userName).subscribe(
      () => {
        this.followMode = false;
        this.user.followersCount ++;

      },
      (error: any) => console.error(error))
      .add(() => this.spinner.hide());
  }

  public unfollowUser(): void{
    if (this.userName === null || this.userName === '') return;
    this.spinner.show();
    this.accountService.unfollowUser(this.userName).subscribe(
      () => {
        this.followMode = true;
        this.user.followersCount --;
      },
      (error: any) => console.error(error))
      .add(() => this.spinner.hide());
  }

  public toggleEditMode(): void{
    this.editMode = !this.editMode;
  }

  openFollowers(): void {
    this.router.navigateByUrl(`/user/profile/${this.userName}/followers`)
  }

  openFollowing(): void {
    this.router.navigateByUrl(`/user/profile/${this.userName}/following`)

  }
}

