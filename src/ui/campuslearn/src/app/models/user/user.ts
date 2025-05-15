export interface User {
  id: string;
  name: string;
  surname: string;
  email: string;
  password: string;
  contactNumber: string;
  role: UserRole;
}


export enum UserRole {
  Admin = 1,
  Lecturer = 2,
  Tutor = 3,
  Student = 4,
}
