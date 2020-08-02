import { Component, OnInit, ViewChild, ChangeDetectorRef, OnChanges, AfterViewInit } from '@angular/core';
import { Chronicle } from 'app/models/chronicle';
import { AuthenticationService } from 'app/services/authentication.service';
import { ChronicleService } from 'app/services/chronicle.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Location } from '@angular/common';
import Swal from 'sweetalert2';
import { NguCarousel, NguCarouselConfig } from '@ngu/carousel';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-add-chronicle',
  templateUrl: './add-chronicle.component.html',
  styleUrls: ['./add-chronicle.component.scss']
})
export class AddChronicleComponent implements OnInit, OnChanges, AfterViewInit {

  // tslint:disable-next-line:max-line-length
  constructor(private cdr: ChangeDetectorRef, private router: Router, private location: Location, private activateRoute: ActivatedRoute, public datepipe: DatePipe, private chronicleService: ChronicleService, private authService: AuthenticationService) {
  }

  chronicle: Chronicle;
  activeParameter: any;
  query: string;

  authenticated: boolean;
  loading = false;
  formData: FormData;

  data: string;
  urls = new Array<string>();

  @ViewChild('myCarousel') myCarousel: NguCarousel<any>;
  carouselConfig: NguCarouselConfig = {
    grid: { xs: 2, sm: 3, md: 3, lg: 5, all: 0 },
      slide: 2,
      speed: 400,
      animation: 'lazy',
      point: {
        visible: true
      },
      load: 2,
      loop: true,
      touch: true,

      easing: 'ease'
    };
  carouselItems = [1, 2, 3];

  files: any;
  usrId: any;
  ngOnInit() {
    this.usrId = JSON.parse(localStorage.getItem('currentUser')).id;
    this.activateRoute.params.subscribe((params: Params) => {
      this.activeParameter = params.id;
    });

    if (this.activeParameter) {
      this.getChronicle();
    } else {
      this.chronicle = new Chronicle();
    }
  }

  ngOnChanges() {
   // this.cdr.detectChanges();
  }

  ngAfterViewInit() {
   // this.cdr.detectChanges();
}

  getChronicle() {
    let counter = 0;
    this.carouselItems = new Array();
    this.chronicleService.getChronicleById(this.activeParameter).subscribe((response: Chronicle) => {
      this.chronicle = { ...response };
      this.chronicle.photos.forEach(photo => {
        this.urls.push(photo.title);
        this.carouselItems.push(counter);
        counter++;
      });
    });
  }

  apply(event: Event) {
    event.preventDefault();
    this.formData.append('Title', this.chronicle.title);
    this.formData.append('userId', this.usrId);
    const d = new Date(this.chronicle.date).toDateString();
    const date = this.datepipe.transform(d, 'yyyy-MM-dd');
    this.formData.append('Date', date);

    if (this.files && this.files.length > 0) {
      // tslint:disable-next-line:prefer-for-of
      for (let i = 0; i < this.files.length; i++) {
        const chosenFile = this.files[i];
        this.formData.append('file', chosenFile);
      }
    }

    if (this.chronicle && !this.chronicle.id) {
      this.chronicleService.uploadFile(this.formData)
        .subscribe(
          (response) => {
            Swal.fire({
              title: 'Pridėta!',
              type: 'success',
              text: 'Metraštis pridėtas sėkmingai',
              confirmButtonText: 'Gerai',
              confirmButtonColor: '#3f7e2e'
            });

            this.router.navigate(['metrasciai']);
          },
          (error) => {
            Swal.fire({
              title: 'Pridėti nepavyko',
              type: 'error',
              text: 'Metraščio pridėti nepavyko',
              confirmButtonText: 'Gerai',
              confirmButtonColor: '#c70909'
            });
          }
        );
    } else if (this.chronicle && this.chronicle.id) {
      this.chronicleService.updateChronicle(this.chronicle.id, this.formData)
        .subscribe(
          (response) => {
            Swal.fire({
              title: 'Atnaujinta!',
              type: 'success',
              text: 'Metraštis atnaujintas sėkmingai',
              confirmButtonText: 'Gerai',
              confirmButtonColor: '#3f7e2e'
            });

            this.router.navigate(['metrasciai']);
          },
          (error) => {
            Swal.fire({
              title: 'Atnaujinti nepavyko',
              type: 'error',
              text: 'Metraščio atnaujinti nepavyko',
              confirmButtonText: 'Gerai',
              confirmButtonColor: '#c70909'
            });
          }
        );
    }
  }

  delete() {
    Swal.fire({
      title: 'Ištrinti metraštį?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#c70909',
      confirmButtonText: 'Patvirtinti',
      cancelButtonText: 'Atšaukti'
    }).then((result) => {
      if (result.value) {
        this.chronicleService.deleteChronicle(this.chronicle.id)
          .subscribe(
            res => {
              Swal.fire({
                title: 'Ištrinta!',
                type: 'success',
                text: 'Metraštis ištrintas sėkmingai',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#3f7e2e'
              });

              this.router.navigate(['metrasciai']);
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

  cancel() {
    this.location.back();
  }

  detectFiles(event) {
    let counter = 0;
    this.carouselItems = new Array();
    this.formData = new FormData();
    this.urls = [];
    this.files = event.target.files;
    if (this.files) {
      for (const file of this.files) {
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.urls.push(e.target.result);
          this.carouselItems.push(counter);
          counter++;
        };
        reader.readAsDataURL(file);
      }
    }
  }

  deletePhotoItem(id) {
    if (this.chronicle && this.chronicle.photos) {
      this.chronicle.photos.splice(id, 1);
    }
    this.carouselItems.splice(id, 1);
    this.urls.splice(id, 1);
  }

  // #endRegion

}
