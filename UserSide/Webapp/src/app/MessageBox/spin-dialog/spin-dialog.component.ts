import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-spin-dialog',
  templateUrl: './spin-dialog.component.html',
  styleUrls: ['./spin-dialog.component.css']
})
export class SpinDialogComponent implements OnInit {

  constructor(private dialog: MatDialogRef<SpinDialogComponent>) {
    this.dialog.disableClose = true;
  }

  ngOnInit() {
  }

}
