import { PageResult } from '../../../core/models/paging/page-result';
import { BasePagedQuery } from '../../../core/models/paging/base-paged-query';

// === QUERIES (READ) ===

/**
 * Query parameters for GET /Trailers
 * Corresponds to: ListTrailerQuery.cs
 */
export class ListTrailersRequest extends BasePagedQuery {
  search?: string | null;
  status?: number | null;

  constructor() {
    super();
  }
}

/**
 * Response item for GET /Trailers
 * Corresponds to: ListTrailerQueryDto.cs
 */
export interface ListTrailerQueryDto {
  id: number;
  licensePlateNumber: string;
  make: string;
  model: string;
  year: number;
  type: string;
  length: number;
  capacity: number;
  registrationExpiration: string;
  insuranceExpiration: string;
  vehicleStatusId: number;
  vehicleStatusName?: string | null;
}

/**
 * Paged response for GET /Trailers
 */
export type ListTrailersResponse = PageResult<ListTrailerQueryDto>;

/**
 * Response for GET /Trailers/{id}
 * Corresponds to: GetTrailerByIdQueryDto.cs
 */
export interface GetTrailerByIdQueryDto {
  id: number;
  licensePlateNumber: string;
  make: string;
  model: string;
  year: number;
  type: string;
  length: number;
  capacity: number;
  registrationExpiration: string;
  insuranceExpiration: string;
  vehicleStatusId: number;
  vehicleStatusName?: string | null;
}

// === COMMANDS (WRITE) ===

/**
 * Command for POST /Trailers
 * Corresponds to: CreateTrailerCommand.cs
 */
export interface CreateTrailerCommand {
  licensePlateNumber: string;
  make: string;
  model: string;
  year: number;
  type: string;
  length: number;
  capacity: number;
  vehicleStatusId: number;
  registrationExpiration: string;
  insuranceExpiration: string;
}

/**
 * Command for PUT /Trailers/{id}
 * Corresponds to: UpdateTrailerCommand.cs
 */
export interface UpdateTrailerCommand {
    id: number;
  licensePlateNumber: string;
  make: string;
  model: string;
  year: number;
  type: string;
  length: number;
  capacity: number;
  registrationExpiration: string;
  insuranceExpiration: string;
}

/**
 * Command for PATCH /Trailers/{id}/status
 * Corresponds to: ChangeTrailerStatusCommand.cs
 */
export interface ChangeTrailerStatusCommand {
  vehicleStatusId: number;
}
