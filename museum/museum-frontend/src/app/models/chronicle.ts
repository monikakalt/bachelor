import { Photo } from './photo';

export class Chronicle {
    id: number;
    title: string;
    date: Date;
    folderUrl: string;
    fkUser: number;
    photos: Photo[];
}
