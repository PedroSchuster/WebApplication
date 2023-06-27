import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { Constants } from '../util/constants';

@Pipe({
  name: 'DateFormatPipe',
})
export class DateTimeFormatPipe extends DatePipe implements PipeTransform {
  override transform(value: any, args?: any): any {
    const dateParts = value?.split(' ');
    if (value != null){
      const datePart = dateParts[0];
      const timePart = dateParts[1];

          const formattedDate = datePart.split('/').reverse().join('/');
          return super.transform(formattedDate, Constants.DATE_FMT);
    }
    return null;
  }
}
