import { Component } from '@angular/core';
import { SearchResultDto } from 'src/app/dataModels/search-result-dto';
import { StationService } from 'src/app/services/stationsService';
import { MatDialog } from '@angular/material/dialog';
import { SpinDialogComponent } from 'src/app/MessageBox/spin-dialog/spin-dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-result-iteam',
  templateUrl: './result-iteam.component.html',
  styleUrls: ['./result-iteam.component.css']
})
export class ResultIteamComponent {

  data: string;
  // result: SearchResultDto;
  result = {
    Count: '1',
    Result: [{
      Options: [{
        Duration: '05:00:00',
        Direction: '1',
        EndStationArrival: 0,
        EndStationId: 2,
        EndStationName: 'Mahalekam Karyalaya',
        StartStationDeparture: 5,
        StartStationName: 'Fort',
        StationId: 1,
        TrainId: 1,
        TrainName: 'as'
      }, {
        Duration: '00:00:00',
        EndStationArrival: 0,
        Direction: '1',
        EndStationId: 2,
        EndStationName: 'Mahalekam Karyalaya ',
        StartStationDeparture: 0,
        StartStationName: 'Fort',
        StationId: 1,
        TrainId: 5,
        TrainName: 'rrrrrrrr '
      }],
      TimeTaken: '5:00:00'
    }]
  };

  constructor(private stationservice: StationService, private dialog: MatDialog, private route: Router) {
    // this.result = stationservice.GetStationsResults();
  }

  GetLiveTraindata(TrainId: number, StationId: number, endStationId: number, direction: number) {
    // console.log(TrainId + ' ' + StationId + ' ' + endStationId + ' ' + direction);

    this.dialog.open(SpinDialogComponent);

    const urlpath = 'trainId=' + TrainId + '&start=' + StationId + '&end=' + endStationId + '&direction=' + direction;
    this.stationservice.setpath(urlpath);
    this.stationservice.GetTrainDetails().then((res: any) => {
      this.route.navigate(['LiveTrain']);
      this.dialog.closeAll();
    }, (reject) => {
      console.log('rejected');
    });
  }

  GetTrainShedule() {
  }
}

// tslint:disable-next-line: max-line-length
// this.webservice.GetData<SearchResultDto>('Search/SearchTrain?startStationId=17&endStationId=15').subscribe((respons: SearchResultDto) => {
//   this.result = respons;
//   console.log(respons);
// });
// this.result = this.stationservice.GetStationsResulr();
