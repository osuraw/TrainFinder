import { Pipe, PipeTransform } from '@angular/core';
import { Station } from './dataModels/stationModel';

@Pipe({
  name: 'filterPipe'
})
export class FilterPipePipe implements PipeTransform {

  transform(value: Station[], filtertext: string): Station[] {
    if (filtertext === '') {
      return value;
    } else {
      return value.filter(station => station.Name.toLowerCase().includes(filtertext.toLowerCase()));
    }
  }

}
