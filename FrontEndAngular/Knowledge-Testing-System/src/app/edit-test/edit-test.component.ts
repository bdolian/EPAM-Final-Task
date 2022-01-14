import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Test } from '../models/test';
import { Constants } from '../static-files/constants';

@Component({
  selector: 'app-edit-test',
  templateUrl: './edit-test.component.html',
  styleUrls: ['./edit-test.component.css']
})
export class EditTestComponent implements OnInit {

  Test: Test;
  form: FormGroup;
  

  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.getTestData().subscribe((res: Test) => {
      this.Test = <Test>res;
      this.form = this.formBuilder.group({
        Id: res.id,
        Name: res.name,
        TimeToEnd: res.timeToEnd.toString(),
        Questions: this.formBuilder.array([])
      }); 
      res.questions.forEach((question, questionIndex) => {
        this.Questions.push(this.getQuestion(question.id, res.id, question.text));
        question.options.forEach(element => {
          this.getOptionsFormArray(questionIndex).push(this.getOption(element.id, question.id,element.text, 'false'));
        });
      });
      
    },
    (err) =>{
      console.log(err);   
    });
  }

  getTestData() {
    return this.http.get(Constants.APIPath + 'Test/getTestById', {
      params: new HttpParams().set("id", this.route.snapshot.params.id),
      withCredentials: true
    })
  }

  get Questions() {
    return this.form.get("Questions") as FormArray;
  }

  getOptionsFormArray(questionIndex: number) {
    return this.Questions.controls[questionIndex].get("Options") as FormArray;
  }

  getOptionsControls(questionIndex: number) {
    return this.getOptionsFormArray(questionIndex).controls;
  }

  private getQuestion(id: number, testId: number, text: string) {
    return this.formBuilder.group({
      Id: id,
      TestId: testId,
      Text: [text, Validators.required],
      Options: this.formBuilder.array([])
    })
  }

  private getOption(id: number, questionId: number, text: string, isCorrect?: string) {
    return this.formBuilder.group({
      Id: id,
      QuestionId: questionId,
      Text: [text, Validators.required],
      IsCorrect: [isCorrect, Validators.required]
    })
  }

  cancelOptionTrue(questionIndex: number) {
      this.getOptionsFormArray(questionIndex).controls;
  }

  disableButtons(name: string): void {
    document.getElementsByName(name).forEach(element => {
      element.setAttribute("disabled","true");
    });
  }

  submit(): void {
    console.log(this.form.getRawValue());
    this.http.put(Constants.APIPath + 'Test/editTest', this.form.value, {
      withCredentials: true,
      responseType: 'text' as 'json'
    }).subscribe(() => this.router.navigate(['/']));
  }

}
