export interface GenericAPIResponse<T> {
  status: boolean;
  statusCode: number;
  statusMessage: string;
  statusDetailedMessage?: string;
  body?: T;
}


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

export interface Module {
  id: string;
  code: string;
  name: string;
  status: boolean;
}