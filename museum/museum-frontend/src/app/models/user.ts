export class User {
    id: number;
    fullName: string;
    password: string;
    email: string;
    isAdmin?: boolean;
    isActive: boolean;
    token?: string;
}
