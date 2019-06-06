import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SearchComponent } from './search/search.component';
import { SearchstationComponent } from './searchstation/searchstation.component';
import { SearchResultComponent } from './search/search-result/search-result.component';

const route: Routes = [
    { path: '', component: SearchComponent },
    { path: 'TimeTable', component: SearchstationComponent },
    { path: 'SearchResult', component: SearchResultComponent }
];
@NgModule({
    imports: [RouterModule.forRoot(route)],
    exports: [RouterModule]
})

export class AppRouting { }
