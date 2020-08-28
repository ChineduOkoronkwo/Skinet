import { HttpClient } from '@angular/common/http';
import { environment } from './../../../environments/environment';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss']
})
export class TestErrorComponent implements OnInit {
  baseUrl = environment.apiUrl;
  validationErrors: any[];
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

  get404Error(): void {
    this.http.get(`${this.baseUrl}products/42`).subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }

  get500Error(): void {
    this.http.get(`${this.baseUrl}buggy/serverError`).subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }

  get400Error(): void {
    this.http.get(`${this.baseUrl}buggy/badRequest`).subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }

  get400Validation(): void {
    this.http.get(`${this.baseUrl}products/FourtyTwo`).subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
      this.validationErrors = error.errors;
    });
  }

}
