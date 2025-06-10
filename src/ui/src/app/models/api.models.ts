export interface GenericAPIResponse<T> {
  status: boolean;
  statusCode: number;
  statusMessage: string;
  statusDetailedMessage?: string;
  body?: T;
}