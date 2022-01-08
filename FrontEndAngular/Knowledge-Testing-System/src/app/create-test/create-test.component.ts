import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-test',
  templateUrl: './create-test.component.html',
  styleUrls: ['./create-test.component.css']
})
export class CreateTestComponent implements OnInit {
  form: FormGroup;

  constructor(
    private formBuilder: FormBuilder
  ) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      Name: '',
      TimeToEnd: '',
      Questions: this.formBuilder.array([])
    });
  }

  submit(): void {
    console.log(this.form.controls);
  }

  get Questions() {
    return this.form.get("Questions") as FormArray;
  }

  getOptionsFormArray(questionIndex: number){
    return this.Questions.controls[questionIndex].get("Options") as FormArray;
  }

  getOptionsControls(questionIndex: number){
    return this.getOptionsFormArray(questionIndex).controls;
  }

  private getQuestion() {
    return this.formBuilder.group({
      Text: ['', Validators.required],
      Options: this.formBuilder.array([])
    })
  }

  private getOption() {
    return this.formBuilder.group({
      Text: ['', Validators.required]
    })
  }

  addQuestion(): void {
    this.Questions.push(this.getQuestion());
  }

  addOption(questionIndex: number): void {
    this.getOptionsFormArray(questionIndex).push(this.getOption());
  }

  removeQuestion(index: number): void{
    this.Questions.removeAt(index);
  } 

  removeOption(questionIndex: number, index: number): void{
    this.getOptionsFormArray(questionIndex).removeAt(index);
  }
}
