import { Location, LocationStrategy, PathLocationStrategy } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject } from '@angular/core';  
import { DOCUMENT } from '@angular/common';
interface URL {
  url: string
}
@Component({
  selector: 'app-url-redirect',
  providers: [Location, { provide: LocationStrategy, useClass: PathLocationStrategy }],
  templateUrl: './url-redirect.component.html',
  styleUrls: ['./url-redirect.component.css']
})


export class UrlRedirectComponent implements OnInit {
  shortUrl: string
  
  constructor(private location: Location, private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') private baseUrl, @Inject(DOCUMENT) private document: Document) {
    this.baseUrl = baseUrl+'URL'
   }

  ngOnInit(): void {
    this.shortUrl = this.route.snapshot.paramMap.get("shortUrl")
    this.translateURL(this.shortUrl)
  }



  translateURL(url) {

    const header = new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' });
    this.http.get<URL>(this.baseUrl+'/'+this.shortUrl, { headers: header }).subscribe(result => {
     
     let eurl = result.url
     this.document.location.href = eurl
    }, error => console.error(error));

  }
}
