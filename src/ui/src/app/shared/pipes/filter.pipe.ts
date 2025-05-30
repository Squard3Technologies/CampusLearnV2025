import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter',
  standalone: true
})
export class FilterPipe implements PipeTransform {
  transform<T>(items: T[], property: keyof T, value: any): T[] {
    if (!items || !property || value === undefined || value === null) {
      return items || [];
    }

    return items.filter(item => item[property] === value);
  }
}
