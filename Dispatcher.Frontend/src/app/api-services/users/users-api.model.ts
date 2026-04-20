import { BasePagedQuery } from "../../core/models/paging/base-paged-query";
import { PageResult } from "../../core/models/paging/page-result";

/*
 * Response item for GET /Users
 * Corresponds to: ListUserQueryDto.cs
 */
export type ListUsersQueryDto = {
  id: number;
  firstName: string;
  lastName: string;
  displayName: string;
  email: string;
  phoneNumber?: string | null;
  dateOfBirth?: Date | null;
  role: string;
  isEnabled: boolean;
  cityId?: number | null;
  cityName?: string | null;
  profilePhotoUrl?: string | null;
};

/*
 * Paged response for GET /Users
 */
export type ListUsersResponse = PageResult<ListUsersQueryDto>;

/**
 * Query parameters for GET /Users
 * Corresponds to: ListUserQuery.cs
 */
export class ListUsersRequest extends BasePagedQuery {
  search?: string | null;
  role?: string | null;
}


