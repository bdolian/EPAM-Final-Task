import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Emitters } from '../emitters/emitters';
import { Constants } from '../static-files/constants';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  
  TestsList: any;

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.getTests();
    this.http.get(Constants.APIPath + 'User/me', {withCredentials: true}).subscribe(
      (res: any) => {
        console.log(res);
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

  onSelectTest(test): void{
    this.router.navigate(['/tests', test.id])
  }

  createTest(): void {
    this.router.navigate(['/test/create'])
  }

}
