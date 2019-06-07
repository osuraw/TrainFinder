import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { FlexLayoutModule } from '@angular/flex-layout';
import { AgmCoreModule } from '@agm/core';

import { AppRouting } from './app.routing.module';
import { MatirialModel } from './material.module';

import { AppComponent } from './app.component';
import { SearchComponent } from './search/search.component';
import { NavigationComponent } from './navigation/navigation.component';
import { SearchstationComponent } from './searchstation/searchstation.component';
import { StationService } from './services/stationsService';
import { FilterPipePipe } from './filter-pipe.pipe';
import { SearchResultComponent } from './search/search-result/search-result.component';
import { ResultIteamComponent } from './search/search-result/result-iteam/result-iteam.component';
import { ErrorDialogComponent } from './MessageBox/error-dialog/error-dialog.component';
import { SpinDialogComponent } from './MessageBox/spin-dialog/spin-dialog.component';
import { LiveViewComponent } from './search/search-result/result-iteam/live-view/live-view.component';
import { TrainStopsComponent } from './search/search-result/result-iteam/train-stops/train-stops.component';

@NgModule({
  declarations: [
    AppComponent,
    SearchComponent,
    NavigationComponent,
    SearchstationComponent,
    SearchResultComponent,
    FilterPipePipe,
    ResultIteamComponent,
    ErrorDialogComponent,
    SpinDialogComponent,
    LiveViewComponent,
    TrainStopsComponent
  ],
  imports: [
    BrowserModule,
    MatirialModel,
    FlexLayoutModule,
    AppRouting,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    AgmCoreModule.forRoot({ apiKey: 'AIzaSyCg1ETphmUzhfwFBR0P1J_gwMxJEKbrYhk' })
  ],
  providers: [
    StationService
  ],
  bootstrap: [AppComponent],
  entryComponents: [
    ErrorDialogComponent,
    SpinDialogComponent
  ]
})
export class AppModule { }
