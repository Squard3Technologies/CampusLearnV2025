export interface SystemUser {
  id: string;
  firstName: string;
  middleName: string | null;
  surname: string;
  emailAddress: string;
  password: string;
  contactNumber: string;
  role: number;
  roleDescription: string;
  accountStatusId: string;
  accountStatusDescription: string;
}