export type TruckDto = {
  id: number;
  licensePlateNumber: string;
  vinNumber: string;
  make: string;
  model: string;
  year: number;
  capacity: number;
  lastMaintenanceDate: string | null;
  nextMaintenanceDate: string | null;
  registrationExpiration: string | null;
  insuranceExpiration: string | null;
  gpsDeviceId: string | null;
  vehicleStatusId: number;
  vehicleStatusName: string | null;
  engineCapacity: number;
  kw: number;
};

export type CreateTruckRequest = {
  licensePlateNumber: string;
  vinNumber: string;
  make: string;
  model: string;
  year: number;
  capacity: number;
  lastMaintenanceDate: string | null;
  nextMaintenanceDate: string | null;
  registrationExpiration: string | null;
  insuranceExpiration: string | null;
  gpsDeviceId: string | null;
  vehicleStatusId: number;
  engineCapacity: number;
  kw: number;
};

export type UpdateTruckRequest = {
  id: number;
  licensePlateNumber: string;
  vinNumber: string;
  make: string;
  model: string;
  year: number;
  capacity: number;
  lastMaintenanceDate: string | null;
  nextMaintenanceDate: string | null;
  registrationExpiration: string | null;
  insuranceExpiration: string | null;
  gpsDeviceId: string | null;
  vehicleStatusId: number;
  engineCapacity: number;
  kw: number;
};