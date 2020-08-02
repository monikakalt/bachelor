import { Component, OnInit, ViewChild, OnChanges } from '@angular/core';
import { Router } from '@angular/router';
import { ClassInfo } from 'app/models/classInfo';
import { ClassInfoService } from 'app/services/classes.service';
import Swal from 'sweetalert2';
import { AgGridNg2 } from 'ag-grid-angular';
import { GridOptions } from 'ag-grid-community';
import { Teacher } from 'app/models/teacher';
import { TeachersService } from 'app/services/teachers.service';

@Component({
  selector: 'app-classes',
  templateUrl: './classes.component.html',
  styleUrls: ['./classes.component.scss']
})
export class ClassesComponent implements OnInit, OnChanges {
  @ViewChild('agGrid') agGrid: AgGridNg2;
  public gridOptions: GridOptions;
  classes$: ClassInfo[];
  usrId: any;

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
  teachers: Teacher[];

  constructor(private router: Router, private classService: ClassInfoService, private teachersService: TeachersService) {
    this.gridOptions = {} as GridOptions;
    this.getTeachers();
  }

  ngOnInit() {
    this.usrId = JSON.parse(localStorage.getItem('currentUser')).id;
    this.getClasses();
    this.initializeColumnDefs();
  }

  ngOnChanges() {
    if (this.gridOptions && this.gridOptions.api) {
      this.gridOptions.api.sizeColumnsToFit();
    }
  }

  getClasses() {
    this.classService.getClasses().subscribe((res: ClassInfo[]) => {
      this.classes$ = res;
    });
  }

  getTeachers() {
    this.teachersService.getTeachers().subscribe((response: Teacher[]) => {
      this.teachers = response;
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
        headerName: 'Klasė',
        colId: 'title',
        field: 'title',
        resizable: true,
        sortable: true,
        hide: false
      },
      {
        headerName: 'Klasės vadovas',
        colId: 'fkTeacher',
        field: 'fkTeacher',
        sortable: true,
        resizable: true,
        hide: false,
        valueFormatter: (params: any) => {
          if (!params || !params.value || !this.teachers || this.teachers.length === 0) {
            return '';
          }
          const val = this.teachers.find(x => x.id === params.value);
          if (val && val.fullName) {
            return val.fullName;
          }
        }
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
            ' title="Redaguoti klasę"><img height="15" src="../../../assets/icons/edit.svg"></span>';
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
            'style="cursor: pointer; float: left" title="Ištrinti klasę">' +
            '<img height="15" src="../../../assets/icons/trashcan.svg"></span>';
        }
      },
    );
  }

  sizeToFit() {
    this.gridOptions.api.sizeColumnsToFit();
    this.gridApi.sizeColumnsToFit();
  }

  onGridReady(params) {
    this.gridApi = params.api;
    this.gridApi.sizeColumnsToFit();
  }

  onCellClicked($event: any) {
    if (!$event || !$event.event || !$event.event.target || !$event.event.target.attributes['x-action']) {
      return;
    }

    const action: string = $event.event.target.attributes['x-action'].value;

    switch (action) {
      case 'delete':
        this.deleteClass($event.data.id);
        break;
      case 'edit':
        this.edit($event.data.id);
        break;

      default:
        break;
    }
  }

  add() {
    this.router.navigate(['nauja-klase']);
  }

  edit(id: number) {
    this.router.navigate(['klase/' + id]);
  }

  deleteClass(id) {
    const c = this.classes$.find(x => x.id === id);
    Swal.fire({
      title: 'Ištrinti klasę?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#c70909',
      confirmButtonText: 'Patvirtinti',
      cancelButtonText: 'Atšaukti'
    }).then((result) => {
      if (result.value) {
        this.classService.deleteClass(id)
          .subscribe(
            res => {
              Swal.fire({
                title: 'Ištrinta!',
                type: 'success',
                text: 'Klasės informacija ištrinta',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#3f7e2e'
              });

              const index = this.classes$.indexOf(c, 0);
              if (index > -1) {
                this.classes$.splice(index, 1);
                this.getClasses();
              }
            },
            error => {
              Swal.fire({
                title: 'Ištrinti nepavyko',
                type: 'error',
                text: 'Klasės informacijos ištrinti nepavyko',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#c70909'
              });
            });
      }
    });
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
