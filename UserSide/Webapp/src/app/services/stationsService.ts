import { Injectable } from '@angular/core';
import { Station } from '../dataModels/stationModel';
import { SearchResultDto } from '../dataModels/search-result-dto';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable()
export class StationService {

    private url = 'http://localhost:11835/Api/';
    public result: SearchResultDto;
    public stations: Station[];
    constructor(private http: HttpClient) { }

    Oncall(url: string) {
        const promise = new Promise((resolve, reject) => {
            setTimeout(() => { }, 5000);
            this.http.get<SearchResultDto>(this.url + url)
                .toPromise()
                .then(
                    res => {
                        this.result = res;
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

    GetStations(path: string): Observable<Station[]> {
        return this.http.get<Station[]>(this.url + path);
    }

    GetStationsResults() {
        return this.result;
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
