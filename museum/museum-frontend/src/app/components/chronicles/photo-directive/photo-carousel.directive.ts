import { Directive, ElementRef, Input, HostListener } from '@angular/core';
import { Photo } from 'app/models/photo';
import { Chronicle } from 'app/models/chronicle';

@Directive({
  selector: '[appPhotoCarousel]'
})
export class PhotoCarouselDirective {

  constructor(private el: ElementRef) { }

  @Input('appPhotoCarousel')
  photos: Photo[];

  @Input()
  index: number;

  @HostListener('click') onClick() {
    this.showPhoto(this.index);
  }

  @HostListener('change') OnChange() {
    this.isActive(this.index);
  }

// show a certain image
showPhoto(index) {
  this.index = index;
}

  showPrev() {
    this.index = (this.index > 0) ? --this.index : this.photos.length - 1;
  }

    // if a current image is the same as requested image
  isActive(index) {
      return this.index === index;
    }

}
