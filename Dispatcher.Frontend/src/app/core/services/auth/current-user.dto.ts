export enum UserRole {
  Client = 0,
  Driver = 1,
  Dispatcher = 2,
  Admin = 3
}

export interface CurrentUserDto {
  userId: number;
  email: string;
  role: UserRole;
  tokenVersion: number;
}
