import { Component } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { Subject, debounceTime } from 'rxjs';
import { ValidatorField } from 'src/app/helpers/ValidatorField';
import { User } from 'src/app/models/identity/User';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent {

  user = {} as User;
  form!: FormGroup; // esse ! eh pra n ser inicializada aki

  userNameChanged: Subject<string> = new Subject<string>();

  get f(): any { return this.form.controls; }

  constructor(public fb: FormBuilder,
    private accountService: AccountService,
    private router: Router,
    private toaster: ToastrService) {}

  ngOnInit(): void{
    this.validation();
    console.log(this.f.password.errors);
  }

  private validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmePassword')
    };

    this.form = this.fb.group({
      userName: ['', [Validators.required, Validators.maxLength(10)]],
      firstName: ['', [Validators.required, Validators.maxLength(10)]],
      lastName: ['', [Validators.required, Validators.maxLength(15)]],
      password: ['', [Validators.minLength(6), Validators.maxLength(30)]],
      email: ['', [Validators.required, Validators.email]],
      confirmePassword: ['', Validators.required],
    }, formOptions);
  }

  public register(): void{
    this.user = {...this.form.value};
    this.accountService.register(this.user).subscribe(
      () => this.router.navigateByUrl('/login'),
      (error: any) => this.toaster.error(error.error)
    );
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

}
