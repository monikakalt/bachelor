import { Component, OnInit, ViewChild } from '@angular/core';
import { Graduates } from 'app/models/graduates';
import { GraduatesService } from 'app/services/graduates.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { AgGridNg2 } from 'ag-grid-angular';
import { GridOptions } from 'ag-grid-community';

@Component({
  selector: 'app-graduates',
  templateUrl: './graduates.component.html',
  styleUrls: ['./graduates.component.scss']
})
export class GraduatesComponent implements OnInit {
  @ViewChild('agGrid') agGrid: AgGridNg2;
  public gridOptions: GridOptions;

  columnDefs: any[];
  rowSelection = 'single';
  readOnly = true;
  enableSorting = true;
  fillViewport = 200;
  localeText = {
    page: 'Puslapis',
    to: 'iki',
    of: 'iš',
    more: 'daugiau',
    next: 'kitas',
    last: 'paskutinis',
    first: 'pirmas',
    previous: 'atgal',
  };

  query: any;
  private gridApi;

  constructor(private router: Router, private gradService: GraduatesService) {
    this.gridOptions = {} as GridOptions;
   }

  graduates$: Graduates[];

  ngOnInit() {
    this.getGrads();
    this.initializeColumnDefs();
  }

  getGrads() {
    this.gradService.getGraduates().subscribe((res: Graduates[]) => {
      this.graduates$ = res;
    });
  }

  add() {
    this.router.navigate(['nauja-laida']);
  }

  edit(id: number) {
    this.router.navigate(['laida/' + id]);
  }

  delete(id: number) {
    const teacher = this.graduates$.find(x => x.id === id);
    Swal.fire({
      title: 'Ištrinti laidą?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#c70909',
      confirmButtonText: 'Patvirtinti',
      cancelButtonText: 'Atšaukti'
    }).then((result) => {
      if (result.value) {
        this.gradService.deleteGraduates(id)
          .subscribe(
            res => {
              Swal.fire({
                title: 'Ištrinta!',
                type: 'success',
                text: 'Laidos informacija ištrinta',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#3f7e2e'
              });

              const index = this.graduates$.indexOf(teacher, 0);
              if (index > -1) {
                this.graduates$.splice(index, 1);
                this.getGrads();
              }
            },
            error => {
              Swal.fire({
                title: 'Ištrinti nepavyko',
                type: 'error',
                text: 'Laidos informacijos ištrinti nepavyko',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#c70909'
              });
            });
      }
    });
  }

  initializeColumnDefs() {
    this.columnDefs = [];
    this.columnDefs.push(
      {
        headerName: 'ID',
        colId: 'id',
        field: 'id',
        width: 50,
        resizable: true,
        sortable: true,
        hide: true
      },
      {
        headerName: 'Laida',
        colId: 'title',
        field: 'title',
        resizable: true,
        sortable: true,
        hide: false
      },
      {
        headerName: 'Laidos metai',
        colId: 'year',
        field: 'year',
        resizable: true,
        sortable: true,
        hide: false
      },
      {
        headerName: 'Redaguoti',
        colId: '',
        field: '',
        resizable: false,
        width: 40,
        hide: false,
        cellRenderer: (params: any) => {
          return '<span x-action="edit" class="input-group-text" id="basic-addon1" '
            + 'style="cursor: pointer; float: left; margin-left: 10px;"' +
            ' title="Redaguoti mokytoją"><img height="15" src="../../../assets/icons/edit.svg"></span>';
        }
      },
      {
        headerName: 'Ištrinti',
        colId: '',
        field: '',
        width: 40,
        resizable: false,
        hide: false,
        cellRenderer: (params: any) => {
          return '<span x-action="delete" class="input-group-text" id="basic-addon1"' +
            'style="cursor: pointer; float: left" title="Ištrinti mokytoją">' +
            '<img height="15" src="../../../assets/icons/trashcan.svg"></span>';
        }
      },
    );
  }

  onCellClicked($event: any) {
    if (!$event || !$event.event || !$event.event.target || !$event.event.target.attributes['x-action']) {
      return;
    }

    const action: string = $event.event.target.attributes['x-action'].value;

    switch (action) {
      case 'delete':
        this.delete($event.data.id);
        break;
      case 'edit':
        this.edit($event.data.id);
        break;

      default:
        break;
    }
  }

  sizeToFit() {
    this.gridOptions.api.sizeColumnsToFit();
    this.gridApi.sizeColumnsToFit();
  }

  onGridReady(params) {
    this.gridApi = params.api;
    this.gridApi.sizeColumnsToFit();
  }
  // #region [Search]-----------------------------------------
  public onQuickFilterChanged($event) {
    this.query = $event.target.value;
    this.gridOptions.api.setQuickFilter($event.target.value);
  }

  clearSearch() {
    this.gridOptions.api.setQuickFilter(null);
    this.gridOptions.api.resetQuickFilter();
    this.query = null;
  }
  // #endregion
}
