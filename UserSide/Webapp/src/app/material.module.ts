import { NgModule } from '@angular/core';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
    MatDialogModule,
    MatAutocompleteModule,
    MatInputModule,
    MatFormFieldModule,
    MatListModule,
    MatSidenavModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatToolbarModule,
    MatProgressSpinnerModule,
    MatMenuModule
} from '@angular/material';

@NgModule(
    {
        imports: [
            BrowserAnimationsModule,
            MatCardModule,
            MatToolbarModule,
            MatButtonModule,
            MatIconModule,
            MatSidenavModule,
            MatListModule,
            MatFormFieldModule,
            MatInputModule,
            MatAutocompleteModule,
            MatDialogModule,
            MatProgressSpinnerModule,
            MatMenuModule
        ],
        exports: [
            BrowserAnimationsModule,
            MatCardModule,
            MatToolbarModule,
            MatButtonModule,
            MatIconModule,
            MatSidenavModule,
            MatListModule,
            MatFormFieldModule,
            MatInputModule,
            MatAutocompleteModule,
            MatDialogModule,
            MatProgressSpinnerModule,
            MatMenuModule
        ]
    })

export class MatirialModel { }
