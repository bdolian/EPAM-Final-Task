import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { Constants } from '../static-files/constants';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  form: FormGroup;
  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private router: Router
    ) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      FirstName: '',
      LastName: '',
      Email: '',
      Password: '',
      ConfirmPassword: ''
    });
  }

  submit(): void {
    this.http.post(Constants.APIPath + 'Account/register', this.form.getRawValue(),
    {
      headers: new HttpHeaders({
           'Content-Type':  'application/json',
         })
    })
    .subscribe(res => {
      this.router.navigate(['/login']);
    });
  }

  

}
