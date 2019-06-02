import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchResultComponent implements OnInit {
  start: string;
  end: string;
  date: string;
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    // this.start = this.route.snapshot.queryParams[0];
    // this.end = this.route.snapshot.queryParams[1];
    const pipe = new DatePipe('en-US');
    const now = Date.now();
    this.date = pipe.transform(now, 'short');

    this.route.queryParams.subscribe((param: any) => {
      this.start = param.start;
      this.end = param.end;
    });

  }

}
