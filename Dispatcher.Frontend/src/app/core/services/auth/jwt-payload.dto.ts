// payload kako dolazi iz JWT-a
export interface JwtPayloadDto {
  sub: string;
  email: string;
  role:string; // "Admin" | "Dispatcher" | "Driver" | "Client"
  ver: string;
  iat: number;
  exp: number;
  aud: string;
  iss: string;
}
