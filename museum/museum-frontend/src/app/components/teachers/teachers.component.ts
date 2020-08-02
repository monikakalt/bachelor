import { Component, OnInit, ViewChild, OnChanges } from '@angular/core';
import { Router } from '@angular/router';
import { TeachersService } from 'app/services/teachers.service';
import { Teacher } from 'app/models/teacher';
import Swal from 'sweetalert2';
import { AgGridNg2 } from 'ag-grid-angular';
import { GridOptions } from 'ag-grid-community';

@Component({
  selector: 'app-teachers',
  templateUrl: './teachers.component.html',
  styleUrls: ['./teachers.component.scss']
})
export class TeachersComponent implements OnInit, OnChanges {
  @ViewChild('agGrid') agGrid: AgGridNg2;
  public gridOptions: GridOptions;
  teachers$: Teacher[];

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

  constructor(private router: Router, private teacherService: TeachersService) {
    this.gridOptions = {} as GridOptions;
  }

  ngOnInit() {
    this.getTeachers();
    this.initializeColumnDefs();
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
        headerName: 'Mokytojo vardas ir pavardė',
        colId: 'fullName',
        field: 'fullName',
        resizable: true,
        sortable: true,
        hide: false
      },
      {
        headerName: 'Dalykas',
        colId: 'subject',
        field: 'subject',
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
        this.deleteTeacher($event.data.id);
        break;
      case 'edit':
        this.editTeacher($event.data.id);
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

  ngOnChanges() {
    if (this.gridOptions && this.gridOptions.api) {
      this.gridOptions.api.sizeColumnsToFit();
    }
  }

  getTeachers() {
    this.teacherService.getTeachers().subscribe((res: Teacher[]) => {
      this.teachers$ = res;
    });
  }

  editTeacher(id) {
    this.router.navigate(['mokytojas/' + id]);
  }

  deleteTeacher(id) {
    const teacher = this.teachers$.find(x => x.id === id);
    Swal.fire({
      title: 'Ištrinti mokytoją?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#c70909',
      confirmButtonText: 'Patvirtinti',
      cancelButtonText: 'Atšaukti'
    }).then((result) => {
      if (result.value) {
        this.teacherService.deleteTeacher(id)
          .subscribe(
            res => {
              Swal.fire({
                title: 'Ištrinta!',
                type: 'success',
                text: 'Mokytojo informacija ištrinta',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#3f7e2e'
              });

              const index = this.teachers$.indexOf(teacher, 0);
              if (index > -1) {
                this.teachers$.splice(index, 1);
                this.getTeachers();
              }
            },
            error => {
              Swal.fire({
                title: 'Ištrinti nepavyko',
                type: 'error',
                text: 'Mokytojo informacijos ištrinti nepavyko',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#c70909'
              });
            });
      }
    });
  }

  add() {
    this.router.navigate(['naujas-mokytojas']);
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
