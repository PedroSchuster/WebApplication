import { Component } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
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
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      userName: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
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


}
