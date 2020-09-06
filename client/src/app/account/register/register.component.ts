import { AccountService } from './../account.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AsyncValidatorFn } from '@angular/forms';
import { Router } from '@angular/router';
import { timer, of } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  errors: string[];

  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.createRegisterForm();
  }
  createRegisterForm(): void {
    this.registerForm = this.fb.group({
      displayName: [null, [Validators.required]],
      email: [null, [Validators.required,
        Validators.pattern('^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$')],
        [this.validateEmailNotTaken()]],
     password: [null, [Validators.required]]
    });
  }

  onSubmit(): void {
    this.accountService.register(this.registerForm.value).subscribe(() => {
      this.router.navigateByUrl('/shop');
    }, error => {
      console.log(error);
      this.errors = error.errors;
    });
  }

  validateEmailNotTaken(): AsyncValidatorFn {
    console.log('validateEmailNotTaken');
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if (!control.value) {
            return of(null);
          }
          return this.accountService.checkEmailExist(control.value).pipe(
            map(res => {
              return res ? {emailExists: true} : null;
            })
          );
        })
      );
    };
  }

}
