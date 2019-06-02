import { Component, OnInit } from '@angular/core';
import { StationService } from './services/stationsService';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Train Finder';
  constructor(private stationService: StationService) { }

  ngOnInit(): void {
    this.stationService.GetStations('Search/GetStations');
  }
}
