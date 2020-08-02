import { Component, ViewChild, OnChanges, AfterViewInit, OnDestroy, ChangeDetectorRef, OnInit } from '@angular/core';
import { StudentService } from 'app/services/students.service';
import { Student } from '../../models/student';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { AgGridNg2 } from 'ag-grid-angular';
import { GridOptions } from 'ag-grid-community';
import { TeachersService } from 'app/services/teachers.service';
import { ClassInfoService } from 'app/services/classes.service';
import { ClassInfo } from 'app/models/classInfo';
import { Teacher } from 'app/models/teacher';
import { GraduatesService } from 'app/services/graduates.service';
import { Graduates } from 'app/models/graduates';
import { ModalService } from 'app/services/modal.service';

@Component({
  selector: 'app-students',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.scss']
})
export class StudentsComponent implements OnInit, AfterViewInit, OnDestroy {

  @ViewChild('agGrid') agGrid: AgGridNg2;
  public gridOptions: GridOptions;

  classes: ClassInfo[] = [];
  teachers: Teacher[] = [];
  graduates: Graduates[] = [];

  savedColumnState: any;

  constructor(private datePipe: DatePipe, private router: Router, private cdr: ChangeDetectorRef,
    private studentService: StudentService, private gradService: GraduatesService,
    private teachersService: TeachersService,
    private modalService: ModalService,
    private classesService: ClassInfoService) {
    this.getTeachers();
    this.getClasses();
    this.getGrads();
    // we pass an empty gridOptions in, so we can grab the api out
    this.gridOptions = {} as GridOptions;
    this.studentService.getStudents().subscribe((res: Student[]) => {
      this.students = res;
      this.model = JSON.parse(JSON.stringify(this.students));
    });
    this.initializeColumnDefs();
  }

  ngOnInit() {
  }

  chosenFileName: string;
  chosenFile: any;

  // model we'll use for grid
  model: Student[] = [];

  students: Student[] = [];

  public hasBaseDropZoneOver = false;
  public hasAnotherDropZoneOver = false;

  columnDefs: any[];
  rowSelection = 'single';
  readOnly = true;
  enableColResize = true;
  autoSizeColumns = true;
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

  ngOnDestroy() {

  }

  getTeachers() {
    this.teachersService.getTeachers().subscribe((response: Teacher[]) => {
      this.teachers = response;
    });
  }

  getClasses() {
    this.classesService.getClasses().subscribe((response: ClassInfo[]) => {
      this.classes = response;
    });
  }

  getGrads() {
    this.gradService.getGraduates().subscribe((response: Graduates[]) => {
      this.graduates = response;
    });
  }

  ngAfterViewInit() {
    if (!localStorage.getItem('columnState')) {
      this.savedColumnState = this.gridOptions.columnDefs;
    } else {
      const local = JSON.parse(localStorage.getItem('columnState'));
      const columnApi = this.gridOptions.columnApi;
      columnApi.setColumnState(local);
      this.savedColumnState = this.gridOptions.columnDefs;
    }
    this.gridOptions.api.sizeColumnsToFit();
    this.cdr.detectChanges();
  }

  // #region [Data]----------
  getStudents() {
    this.studentService.getStudents().subscribe((response: any) => {
      this.model = { ...response };
      return this.model;
    });
    return this.model;
  }
  // #endregion

  // #region [Students grid]----------------------

  initializeColumnDefs() {
    this.columnDefs = [];
    this.columnDefs.push(
      {
        headerName: 'ID',
        colId: 'id',
        field: 'id',
        width: 50,
        sortable: true,
        hide: true
      },
      {
        headerName: 'Vardas, Pavardė',
        colId: 'fullName',
        field: 'fullName',
        sortable: true,
        width: 160,
        hide: false
      },
      {
        headerName: 'Pavardė po santuokos',
        colId: 'surnameAfterMarriage',
        field: 'surnameAfterMarriage',
        sortable: true,
        width: 150,
        hide: false
      },
      {
        headerName: 'Gimimo data',
        colId: 'birthdate',
        field: 'birthdate',
        width: 100,
        sortable: true,
        hide: false,
        cellRenderer: (params: any) => {
          if (!params || !params.value) {
            return '';
          }
          const val = this.datePipe.transform(params.value, 'yyyy-MM-dd');
          return val;
        }
      },
      {
        headerName: 'Klasė',
        colId: 'fkClass',
        field: 'fkClass',
        width: 58,
        sortable: true,
        hide: false,
        valueFormatter: (params: any) => {
          if (!params || !params.value || !this.classes || this.classes.length === 0) {
            return '';
          }
          const val = this.classes.find(x => x.id === params.value);
          if (val && val.title) {
            return val.title.substring(0, val.title.indexOf('klasė'));
          }
        },
      },
      {
        headerName: 'Darbovietė, mokymosi įstaiga',
        colId: 'workplace',
        field: 'workplace',
        sortable: true,
        hide: false
      },
      {
        headerName: 'Adresas',
        colId: 'address',
        field: 'address',
        sortable: true,
        width: 150,
        hide: false
      },
      {
        headerName: 'Telefono Nr.',
        colId: 'phone',
        field: 'phone',
        sortable: true,
        width: 120,
        hide: false
      },
      {
        headerName: 'El. paštas',
        colId: 'email',
        field: 'email',
        sortable: true,
        hide: false
      },
      {
        headerName: 'Klasės vadovas',
        colId: 'fkClassNavigationFkTeacher',
        field: 'fkClassNavigationFkTeacher',
        sortable: true,
        width: 160,
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
        headerName: 'Laida',
        colId: 'fkGraduates',
        field: 'fkGraduates',
        sortable: true,
        width: 58,
        hide: false,
        valueFormatter: (params: any) => {
          if (!params || !params.value || !this.graduates || this.graduates.length === 0) {
            return '';
          }
          const val = this.graduates.find(x => x.id === params.value);
          if (val && val.title) {
            return val.title.substring(0, val.title.indexOf('laida'));
          }
        }
      },
      {
        headerName: 'Baigimo metai',
        colId: 'fkGraduates',
        field: 'fkGraduates',
        sortable: true,
        width: 120,
        hide: false,
        valueFormatter: (params: any) => {
          if (!params || !params.value || !this.graduates || this.graduates.length === 0) {
            return '';
          }
          const val = this.graduates.find(x => x.id === params.value);
          if (val && val.year) {
            return val.year;
          }
        }
      },
      {
        headerName: 'Pastabos',
        colId: 'comment',
        field: 'comment',
        sortable: true,
        hide: false
      },
    );
  }

  onGridReady(params) {
    this.gridApi = params.api;
    this.gridApi.sizeColumnsToFit();
  }

  cellDoubleClicked(event: any) {
    this.router.navigate(['mokinys/' + event.data.id]);
  }

  add() {
    this.router.navigate(['naujas-mokinys']);
  }

  // #endregion

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

  // #region [File upload]--------------------------------------

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public fileOverAnother(e: any): void {
    this.hasAnotherDropZoneOver = e;
  }

  setChosenFile(fileInput: Event) {
    const control: any = fileInput.target;
    if (!control.files || control.length === 0) {
      this.chosenFileName = null;
      this.chosenFile = null;
    } else {
      this.chosenFileName = control.files[0].name;
      this.chosenFile = control.files[0];
    }
  }

  uploadFile() {
    const uploadData = new FormData();
    uploadData.append('file', this.chosenFile, this.chosenFileName);
    this.studentService
      .uploadFile(uploadData)
      .subscribe(
        (response) => {

        },
        (error) => {

        }
      );
  }


  // #region [Export] -----------------------------

  export() {
    let header = this.columnDefs.map(columnDef => {
      const headerName = columnDef.headerName;
      return headerName;
    });

    header = header.filter(x => x !== 'Baigimo metai');
    const classes = this.classes;
    const teachers = this.teachers;
    const graduates = this.graduates;
    const datePipe = new DatePipe('lt');
    const params: any = {
      fileName: 'export.csv',
      columnSeparator: ';',
      skipHeader: true,
      columnKeys: Array.from(new Set(this.columnDefs.map(c => c.field || c.colId).filter(c => !!c))),
      // format cells before exporting
      processCellCallback(p) {
        if (p && p.value) {
          if (p.column.colId === 'fkClass') {
            const val = classes.find(x => x.id === p.value);
            if (val && val.title) {
              return val.title.substring(0, val.title.indexOf('klasė'));
            }
          }
          if (p.column.colId === 'birthdate') {
            const a = datePipe.transform(p.value, 'yyyy-MM-dd');
            return a;
          }
          if (p.column.colId === 'fkClassNavigationFkTeacher') {
            const t = teachers.find(x => x.id === p.value);
            if (t && t.fullName) {
              return t.fullName;
            }
          }
          if (p.column.colId === 'fkGraduates' && p.column.colDef.headerName === 'Laida') {
            const g = graduates.find(x => x.id === p.value);
            if (g && g.title) {
              return g.title.concat(' (').concat(g.year.toString() + 'm.)');
            }
          }
          return p.value ? p.value : '';
        }
      }
    };
    params.customHeader = header.join(params.columnSeparator) + '\n';
    debugger;
    this.gridOptions.api.exportDataAsCsv(params);
  }

  // #endRegion

  // #Region [Column selection] ---------------------
  openModal(id: string) {
    this.modalService.open(id);
  }

  setColumnVisibility(columnDef: any, visibility: boolean) {
    const columnApi = this.gridOptions.columnApi;
    columnApi.setColumnVisible(columnDef.colId || columnDef.field, visibility);
  }

  closeModal(id: string) {
    this.modalService.close(id);
    this.gridOptions.api.sizeColumnsToFit();
  }

  apply(id: string) {
    this.modalService.close(id);
    const columnApi = this.gridOptions.columnApi;
    const columns = JSON.stringify(columnApi.getColumnState());
    localStorage.setItem('columnState', columns);
    this.gridOptions.api.sizeColumnsToFit();
  }

  // #endRegion


}
