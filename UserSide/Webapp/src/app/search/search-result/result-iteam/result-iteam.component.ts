import { Component } from '@angular/core';
import { SearchResultDto } from 'src/app/dataModels/search-result-dto';
import { StationService } from 'src/app/services/stationsService';

@Component({
  selector: 'app-result-iteam',
  templateUrl: './result-iteam.component.html',
  styleUrls: ['./result-iteam.component.css']
})
export class ResultIteamComponent {

  data: string;
  result: SearchResultDto;

  constructor(private stationservice: StationService) {
    this.result = stationservice.GetStationsResults();
  }
}

// tslint:disable-next-line: max-line-length
// this.webservice.GetData<SearchResultDto>('Search/SearchTrain?startStationId=17&endStationId=15').subscribe((respons: SearchResultDto) => {
//   this.result = respons;
//   console.log(respons);
// });
// this.result = this.stationservice.GetStationsResulr();