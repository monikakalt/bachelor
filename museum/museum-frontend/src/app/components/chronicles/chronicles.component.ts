import { Component, OnInit, PipeTransform, ViewChild, OnChanges } from '@angular/core';
import { Router } from '@angular/router';
import { ChronicleService } from 'app/services/chronicle.service';
import { Chronicle } from 'app/models/chronicle';
import { DomSanitizer } from '@angular/platform-browser';
import { Photo } from 'app/models/photo';
import { trigger, state, style, animate, transition } from '@angular/animations';
import Swal from 'sweetalert2';
import { UserService } from 'app/services/user.service';
import { User } from 'app/models/user';
import { GALLERY_IMAGE, NgxImageGalleryComponent, GALLERY_CONF } from 'ngx-image-gallery';

@Component({
  selector: 'app-chronicles',
  templateUrl: './chronicles.component.html',
  styleUrls: ['./chronicles.component.scss'],
  animations: [
    trigger('changeDivSize', [
      state('initial', style({
        width: '200px',
        height: '200px',
      })),
      state('final', style({
        width: '500px',
        height: '500px',
        opacity: 1
      })),
      transition('initial=>final', animate('900ms')),
      transition('final=>initial', animate('1000ms')),
    ]),
  ]
})
export class ChroniclesComponent implements OnInit, PipeTransform, OnChanges {

  constructor(private router: Router, private chronicleService: ChronicleService,
              private sanitizer: DomSanitizer, private userService: UserService) {
  }

  private static searchLookup: { [id: string]: string } = {};

  chronicles: Chronicle[];
  query: string;
  color: string;
  Url: any;
  isAdmin = false;

  @ViewChild(NgxImageGalleryComponent) ngxImageGallery: NgxImageGalleryComponent;


  currentState = 'initial';

  // #region [Photos]

  index = 0;

  // gallery configuration
  conf: GALLERY_CONF = {
    imageOffset: '1px',
    showDeleteControl: false,
    showImageTitle: false,
  };

  images: GALLERY_IMAGE[] = [];

  openGallery(index: number = 0, photos: Photo[]) {
    this.images = [];
    photos.forEach(p => {
      this.images.push({
        url: '../../../assets/chronicles/' + p.title,
      });
    });
    this.ngxImageGallery.open(index);
  }

  galleryOpened(index) {
  }

  galleryClosed() {
  }

  // callback on gallery image clicked
  galleryImageClicked(index) {
  }

  // callback on gallery image changed
  galleryImageChanged(index) {
  }

  // callback on user clicked delete button
  deleteImage(index) {
  }

  changeState() {
    this.currentState = this.currentState === 'initial' ? 'final' : 'initial';
  }

  clickedOut(event) {
    if (!event.target.className.toString().includes('slide materialboxed')) {
      this.currentState = 'initial';
    }
  }

  ngOnInit() {
    this.checkIfAdmin();
    this.getChronicles();
  }

  ngOnChanges() {
    this.checkIfAdmin();
  }

  checkIfAdmin() {
    if (localStorage.getItem('currentUser')) {
      const localUser = JSON.parse(localStorage.getItem('currentUser'));
      if (localUser && localUser.id) {
        this.userService.getUserById(localUser.id).subscribe((user: User) => {
          this.isAdmin = user.isAdmin;
        });
      }
    }
  }

  getChronicles() {
    this.chronicleService.getChronicles().subscribe((res: Chronicle[]) => {
      this.chronicles = res;
    });
  }

  add() {
    this.router.navigate(['naujas-metrastis']);
  }

  edit(id: any) {
    this.router.navigate(['metrastis/' + id]);
  }

  delete(id) {
    const chronicle = this.chronicles.find(x => x.id === id);
    Swal.fire({
      title: 'Ištrinti metraštį?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#c70909',
      confirmButtonText: 'Patvirtinti',
      cancelButtonText: 'Atšaukti'
    }).then((result) => {
      if (result.value) {
        this.chronicleService.deleteChronicle(id)
          .subscribe(
            res => {
              Swal.fire({
                title: 'Ištrinta!',
                type: 'success',
                text: 'Metraštis ištrintas sėkmingai',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#3f7e2e'
              });

              const index = this.chronicles.indexOf(chronicle, 0);
              if (index > -1) {
                this.chronicles.splice(index, 1);
                this.getChronicles();
              }
            },
            error => {
              Swal.fire({
                title: 'Ištrinti nepavyko',
                type: 'error',
                text: 'Metraščio ištrinti nepavyko',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#c70909'
              });
            });
      }
    });
  }

  onQuickFilterChanged(event) {
    // Execute search
    const result: Chronicle[] = [];

    if (this.query === '' || !this.query) {
      this.getChronicles();
    } else {
      // Full text search (which is relatively expensive to check, and therefore last)
      const parts = this.query.split(' ').filter(q => q && q.trim().length > 0).map(q => q.trim().toLowerCase());
      if (parts.length > 0) {
        for (const ch of this.chronicles) {
          if (this.matchesSearchQuery(ch, parts)) {
            result.push(ch);
          }
        }
      }
    }

    this.chronicles = result;
  }

  private matchesSearchQuery(ch: Chronicle, parts: string[]) {
    const pattern = JSON.stringify(ch);

    if (!pattern) {
      // Data has obviously changed since this component was initialized
      return false;
    }

    let isMatch = true;
    for (const part of parts) {
      if (pattern.indexOf(part) < 0) {
        isMatch = false;
        break;
      }
    }

    return isMatch;
  }

  clearSearch() {
    this.query = null;
  }

  transform(url) {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }
  // if a current image is the same as requested image
  isActive(index) {
    return this.index === index;
  }

  // #endRegion

}
