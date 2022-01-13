import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { element } from 'protractor';
import { Emitters } from '../emitters/emitters';
import { Test } from '../models/test';
import { Constants } from '../static-files/constants';

@Component({
  selector: 'app-tests',
  templateUrl: './tests.component.html',
  styleUrls: ['./tests.component.css']
})
export class TestsComponent implements OnInit {
  public id: number;
  public Test: Test;
  form: FormGroup

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    private formBuilder: FormBuilder
  ) { 
    
  }


  ngOnInit(): void {
    this.id = parseInt(this.route.snapshot.paramMap.get('id'));

    this.form = this.formBuilder.group({
      testId: this.id,
      questionAnswers: this.formBuilder.array([])
    });

    this.http.get(Constants.APIPath + 'Test/getTestById', 
    {
      withCredentials: true, 
      params: new HttpParams().set("id", this.id.toString())
    })
    .subscribe(res => {
      this.Test = <Test>res;
    })
  }

  submit(): void {
    this.http.post(Constants.APIPath + 'Test/passTest', this.form.getRawValue(),
    {
      withCredentials: true,
      responseType: 'text' as 'json'
    }).subscribe(
      (result) => {
        Emitters.testEmitter.emit(this.Test);
        Emitters.resultEmitter.emit(result);
        Emitters.answerEmitter.emit(this.form.getRawValue());
      },
      (err) => {
        console.log(err);
      }
    )
    this.router.navigate(['/tests', this.id, 'result']);
  }

  get questionAnswers() {
    return this.form.get("questionAnswers") as FormArray;
  }

  private getQuestionAnswer(index: number, index2: number) {
    return this.formBuilder.group({
      questionId: index,
      answerId: index2
    })
  }

  addQuestionAnswer(index, index2): void {
    this.questionAnswers.push(this.getQuestionAnswer(index, index2));
  }

  disableButtons(name: string): void {
    document.getElementsByName(name).forEach(element => {
      element.setAttribute("disabled","true");
    });
  }


}
