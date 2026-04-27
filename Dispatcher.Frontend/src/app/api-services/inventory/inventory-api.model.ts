import { BasePagedQuery } from '../../core/models/paging/base-paged-query';
import { PageResult }     from '../../core/models/paging/page-result';

export type InventoryItemDto = {
  id:            number;
  sku:           string;
  name:          string;
  description:   string | null;
  category:      string;
  unitOfMeasure: string;
  unitPrice:     number;
  unitWeight:    number | null;
  unitVolume:    number | null;
  isActive:      boolean;
  photoUrl:      string | null;
  thumbnailUrl:  string | null;
};

export type ListInventoryResponse = PageResult<InventoryItemDto>;

export class ListInventoryRequest extends BasePagedQuery {
  search?:   string | null;
  category?: string | null;
}
