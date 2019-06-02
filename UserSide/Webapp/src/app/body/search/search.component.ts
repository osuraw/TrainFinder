import { Component } from '@angular/core';
import { StationService } from 'src/app/services/stationsService';
import { Station } from 'src/app/dataModels/stationModel';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/MessageBox/error-dialog/error-dialog.component';
import { SpinDialogComponent } from 'src/app/MessageBox/spin-dialog/spin-dialog.component';


@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent {

  options: Station[];
  StartStationId = -1;
  EndStationId = -1;
  private start: string;
  private end: string;


  constructor(private stationservice: StationService, private route: Router, private dialog: MatDialog) {
    this.options = this.stationservice.stations;
    if (this.options === undefined) {
      console.log('options empty');
      this.stationservice.Stationload.subscribe((result: Station[]) => this.Onload(result));
    }
  }
  private Onload(result: Station[]) {
    this.options = result;
  }

  private _filter(value: string): Station[] {
    const valueenter = value.toLowerCase();
    return this.options.filter(station => station.Name.toLowerCase().includes(valueenter));
  }

  onSubmit(form): void {
    // console.log(form);
    const val = this.CheckValidity(form);
    // console.log(val);
    if (val) {
      this.dialog.open(SpinDialogComponent);
      this.SendApiRequest();
    }
  }
  // Validation check
  CheckValidity(form: NgForm): boolean {

    this.start = form.value.startstation;
    this.end = form.value.endstation;
    if (this.start === this.end) {
      this.dialog.open(ErrorDialogComponent, {
        data: {
          title: 'Validation Check',
          content: 'Both Start And End Station Are Same'
        }
      });
      return false;
    }
    this.StartStationId = this.GetID(this.start);
    this.EndStationId = this.GetID(this.end);
    if (this.EndStationId !== 0 && this.StartStationId !== 0) {
      return true;
    } else {
      this.dialog.open(ErrorDialogComponent, {
        data: {
          title: 'Validation Check',
          content: 'Selected Stations Not Valide'
        }
      });
      return false;
    }
  }

  // Get Id
  GetID(sttationName: string): number {
    let Id = 0;
    this.options.find((data: Station) => {
      if (data.Name === sttationName) {
        Id = data.SID;
        return true;
      }
    });
    return Id;
  }

  // send Api Request
  SendApiRequest() {
    const urlpath = 'Search/SearchTrain?startStationId=' + this.StartStationId + '&endStationId=' + this.EndStationId;
    this.stationservice.Oncall(urlpath).then((res: any) => {
      // console.log(res);
      this.route.navigate(['SearchResult'], { queryParams: { start: this.start, end: this.end } });
      this.dialog.closeAll();
    }, (reject) => {

      console.log('rejected');
    });
  }
}





// this.webser.GetData('Search/GetStations').subscribe((respons: Station[]) => {
//   this.options = respons;
//   console.log(respons);
// });

// import { startWith, map } from 'rxjs/operators';
// import { Observable } from 'rxjs';
// options: Station[] = [{ Name: 'Fort', SID: 1 }, { Name: 'Mahalekam Karyalaya', SID: 2 }, { Name: 'Kompanyaweediya', SID: 3 }];
// startstation = new FormControl();
// endstation = new FormControl();
// this.filterstaion = this.startstation.valueChanges.pipe(
//   startWith(''),
//   map(value => this._filter(value))
// );
// this.filterstaion1 = this.endstation.valueChanges.pipe(
//   startWith(''),
//   map(value => this._filter(value))
// );
