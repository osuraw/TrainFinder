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
    MatSpinner
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
            MatSpinner
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
            MatSpinner
        ]
    })

export class MatirialModel { }
