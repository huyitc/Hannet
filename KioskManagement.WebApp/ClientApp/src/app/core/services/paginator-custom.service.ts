import { TranslateService } from '@ngx-translate/core';
import { Injectable } from '@angular/core';
import { MatPaginatorIntl } from '@angular/material/paginator';


@Injectable({
  providedIn: 'root'
})
export class PaginatorCustomService extends MatPaginatorIntl{


  setLable!:string;
  firstButton!:string;
  nextButton!:string;
  lastButton!:string;
  preButton!:string;

  constructor(private translateService: TranslateService) {
    super();
    this.setTitle();
    this.getAndInitTranslations();
   }

   private setTitle(){

    this.translateService.get('messageSystem.itemsPerPageLabel').subscribe((data)=>{
      this.setLable= data;
    });
    this.translateService.get('button.firstButton').subscribe((data)=>{
      this.firstButton= data;
    });
    this.translateService.get('button.nextButton').subscribe((data)=>{
      this.nextButton= data;
    });
    this.translateService.get('button.lastButton').subscribe((data)=>{
      this.lastButton= data;
    });
    this.translateService.get('button.preButton').subscribe((data)=>{
      this.preButton= data;
    });
   }


   getAndInitTranslations() {
    this.translateService.get('messageSystem.itemsPerPageLabel').subscribe((data)=>{
      this.itemsPerPageLabel= data;
    });
    this.translateService.get('button.firstButton').subscribe((data)=>{
      this.firstPageLabel= data;
    });
    this.translateService.get('button.nextButton').subscribe((data)=>{
      this.nextPageLabel= data;
    });
    this.translateService.get('button.lastButton').subscribe((data)=>{
      this.lastPageLabel= data;
    });
    this.translateService.get('button.preButton').subscribe((data)=>{
      this.previousPageLabel= data;
    });
    this.changes.next();
   }

   getRangeLabel = (page: number, pageSize: number, length: number) =>  {
    if (length === 0 || pageSize === 0) {
      return `0 / ${length}`;
    }
    length = Math.max(length, 0);
    const startIndex = page * pageSize;
    const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
    return `${startIndex + 1} - ${endIndex} / ${length}`;
  }
}
