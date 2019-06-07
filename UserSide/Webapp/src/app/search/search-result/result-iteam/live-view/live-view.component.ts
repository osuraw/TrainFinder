import { Component, OnInit, OnDestroy } from '@angular/core';
import { StationService } from 'src/app/services/stationsService';
import { TrainDetails } from '../../../../dataModels/TrainDetails';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-live-view',
  templateUrl: './live-view.component.html',
  styleUrls: ['./live-view.component.css']
})
export class LiveViewComponent implements OnInit, OnDestroy {

  lat: number;
  lng: number;
  title = 'My first AGM project';
  trainDetails: TrainDetails;
  timer: any;
  // TrainDetails = { Status: '5', ETA: '00000', ETD: '00000', TrainName: 'as', Speed: '0.09', Location: '6.807833 79.881568' };

  constructor(private trainservice: StationService, private route: ActivatedRoute) {
    this.trainDetails = this.trainservice.TrainDetails;
    this.setlocation();
    console.log(this.trainDetails);
  }

  ngOnInit() {
    this.timer = setInterval(() => {
      this.trainDetails = this.trainservice.TrainDetails;
      // console.log(this.TrainDetails);
      this.trainservice.GetTrainDetails().then((res: any) => {
        this.setlocation();
      });
    }, 1000);
  }

  private setlocation() {
    const res = this.trainDetails.Location.split(' ');
    this.lat = parseFloat(res[0]);
    this.lng = parseFloat(res[1]);
  }

  ngOnDestroy() {
    clearInterval(this.timer);
  }
}
