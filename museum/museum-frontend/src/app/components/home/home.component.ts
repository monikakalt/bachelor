import { Component, OnInit, ViewChild } from '@angular/core';
import { ChronicleService } from 'app/services/chronicle.service';
import { AuthenticationService } from 'app/services/authentication.service';
import { Chronicle } from 'app/models/chronicle';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(private chronicleService: ChronicleService, private authService: AuthenticationService) {
  }

  chronicle: Chronicle;
  urls: string[] = [];

  ngOnInit() {
    this.getRecentChronicle();
  }

  getRecentChronicle() {
    this.chronicleService.getRecentChronicle().subscribe((response: Chronicle) => {
      this.chronicle = { ...response };
      const folder = this.chronicle.folderUrl;

      // fs.readdir(folder, (err, files) => {
      //   files.forEach(file => {
      //     console.log(file);debugger;
      //   });
      // });
    });
  }

  open() {
    debugger;
  }
}
