import { Component } from '@angular/core';
import { StationService } from 'src/app/services/stationsService';
import { Station } from 'src/app/dataModels/stationModel';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/MessageBox/error-dialog/error-dialog.component';
import { SpinDialogComponent } from "src/app/MessageBox/spin-dialog/spin-dialog.component";


@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent {

  options: Station[];
  loading = false;
  StartStationId = -1;
  EndStationId = -1;

  constructor(private stationservice: StationService, private route: Router, private dialog: MatDialog) {
    this.stationservice.GetStations('Search/GetStations').subscribe((response: Station[]) => {
      this.options = response;
      console.log(response);
    });
  }

  private _filter(value: string): Station[] {
    const valueenter = value.toLowerCase();
    return this.options.filter(station => station.Name.toLowerCase().includes(valueenter));
  }

  onSubmit(form): void {
    console.log(form);
    const val = this.CheckValidity(form);
    console.log(this.StartStationId);
    console.log(this.EndStationId);
    console.log(val);
    if (val) {
      this.loading = true;
      this.dialog.open(SpinDialogComponent);
      this.SendApiRequest();
    }
    console.log(this.loading);
  }
  // Validation check
  CheckValidity(form: NgForm): boolean {

    const start = form.value.startstation;
    const end = form.value.endstation;
    if (start === end) {
      this.dialog.open(ErrorDialogComponent, {
        data: {
          title: 'Validation Check',
          content: 'Both Start And End Station Are Same'
        }
      });
      return false;
    }
    this.StartStationId = this.GetID(start);
    this.EndStationId = this.GetID(end);
    if (this.EndStationId !== 0 && this.StartStationId !== 0) {
      return true;
    }
    else {
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
      console.log(res);
      this.loading = false;
      console.log(this.loading);
      this.route.navigate(['SearchResult']);
    }, (reject) => {
      console.log(reject);
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
