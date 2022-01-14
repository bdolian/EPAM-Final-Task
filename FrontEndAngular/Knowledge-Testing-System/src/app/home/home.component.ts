import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Emitters } from '../emitters/emitters';
import { Constants } from '../static-files/constants';
import { Router } from '@angular/router';
import { Test } from '../models/test';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  
  TestsList: any;
  userEmail: string;
  isAdmin: boolean = false;

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.getTests();
    this.http.get(Constants.APIPath + 'User/me', {withCredentials: true}).subscribe(
      (res: any) => {
        this.userEmail = res.user.email;
        this.checkAdminRole();
        Emitters.authEmitter.emit(true);
      },
      (err: any) => {
        console.log(err);
        Emitters.authEmitter.emit(false);
      }
    )
  }

  getTests(): void {
    this.http.get(Constants.APIPath + 'Test/getTests', {withCredentials: true})
    .subscribe(
      res => { 
        this.TestsList = res;
      }
    )
  }

  checkAdminRole(): void {
    this.http.get(Constants.APIPath + 'Role/getUserRoles', {
      params: new HttpParams().set("email", this.userEmail),
      withCredentials: true
    })
    .subscribe(
      (res: any) => {
        res.forEach(element => {
          if(element == "admin"){
            this.isAdmin = true;
            Emitters.adminRoleEmitter.emit(true);
        }});
        }
    )
  }

  onSelectTest(test): void{
    this.router.navigate(['/tests', test.id])
  }

  createTest(): void {
    this.router.navigate(['/test/create'])
  }

  editTest(id: number, index: number): void {
    Emitters.testEmitter.emit(<Test>this.TestsList[index]);
    this.router.navigate(['/tests', id, 'edit'])
  }

  deleteTest(id: string): void {
    console.log(id);
    this.http.delete(Constants.APIPath + 'Test/deleteTest',{
      params: new HttpParams().set('id', id),
      withCredentials: true
    }).subscribe((res) => {},
      (err) => { console.log(err);
      });
    location.reload();
  }

}
