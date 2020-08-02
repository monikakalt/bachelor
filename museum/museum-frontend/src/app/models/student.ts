import { ClassInfo } from './classInfo';

export class Student {
    id: number;
    fullName: string;
    surnameAfterMarriage: string;
    birthdate: number;
    fkClass: number;
    workplace: string;
    address: string;
    phone: string;
    email: string;
    fkClassNavigation: ClassInfo;
    fkGraduates: string;
    comment: string;
}
