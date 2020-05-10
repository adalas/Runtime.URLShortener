import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject } from '@angular/core';  
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';

interface SURL {
  sUrl: string
}
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls:['./home.component.css']
})


//URL Validation taken from here: https://www.itsolutionstuff.com/post/angular-validation-for-url-exampleexample.html

export class HomeComponent {

  form: FormGroup = new FormGroup({});
  http: HttpClient;
  shortRes: string;
  beURL:string 
  trustedUrl:SafeUrl
  constructor(http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private sanitizer: DomSanitizer, private fb: FormBuilder) {

    this.beURL = baseUrl+'URL/'
    this.http = http
    this.shortRes = null;

    const reg = '(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?';

    this.form = fb.group({

      url: ['', [Validators.required, Validators.pattern(reg)]]

    })
  }
  urlValue:string = "";

  shortenURL(url)
  {
    let input = {URL:url};
    const header = new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' });
    this.http.post<SURL>(this.beURL,JSON.stringify(input),{headers:header}).subscribe(result => {
            this.shortRes = this.baseUrl+'s/'+result.sUrl
            this.trustedUrl = this.sanitizer.bypassSecurityTrustUrl(this.shortRes)
    }, error => console.error(error));

   
  }

  get f() {

    return this.form.controls;

  }


}
