import { Injectable, Output, EventEmitter } from '@angular/core';
import { Station } from '../dataModels/stationModel';
import { SearchResultDto } from '../dataModels/search-result-dto';
import { TrainDetails } from '../dataModels/TrainDetails';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material';
import { SpinDialogComponent } from '../MessageBox/spin-dialog/spin-dialog.component';

@Injectable()
export class StationService {

    @Output() Stationload = new EventEmitter<Station[]>();
    // private url = 'https://trainfinder.azurewebsites.net/Api/';
    private url = 'http://localhost:11835//Api/';
    public result: SearchResultDto;
    public stations: Station[];
    public TrainDetails: TrainDetails;
    grttraindatapat: string;


    constructor(private http: HttpClient, private dialog: MatDialog) { }

    Oncall(url: string) {
        const promise = new Promise((resolve, reject) => {
            // setTimeout(() => { }, 5000);
            this.http.get<SearchResultDto>(this.url + url)
                .toPromise()
                .then(
                    res => {
                        this.result = res;
                        console.log(this.result);
                        resolve();
                    },
                    rej => {

                        console.log(rej);
                        reject();
                    }
                );
        });
        return promise;
    }

    GetStations(path: string): void {
        this.dialog.open(SpinDialogComponent);
        this.http.get<Station[]>(this.url + path).subscribe((response: Station[]) => {

            this.Stationload.emit(response);
            this.stations = response;
            this.dialog.closeAll();
            console.log(response);
        });
    }

    GetStationsResults() {
        return this.result;
    }

    GetTrainDetails() {
        const promise = new Promise((resolve, reject) => {
            // setTimeout(() => { }, 5000);
            console.log(this.grttraindatapat);
            this.http.get<TrainDetails>(this.url + 'Search/GetTrainDetails?' + this.grttraindatapat)
                .toPromise()
                .then(
                    res => {
                        this.TrainDetails = res;
                        console.log(this.result);
                        resolve();
                    },
                    rej => {

                        console.log(rej);
                        reject();
                    }
                );
        });
        return promise;
    }
    setpath(path: string) {
        this.grttraindatapat = path;
    }
}

// tslint:disable-next-line: max-line-length
// this.webservice.GetData<SearchResultDto>('Search/SearchTrain?startStationId=17&endStationId=15').subscribe((respons: SearchResultDto) => {
//     this.result = respons;
//     console.log(respons);
// });

 // this.webservice.GetData<Station[]>('Search/GetStations').subscribe((respons: Station[]) => {
        //     this.stations = respons;
        //     console.log(respons);
        // });
