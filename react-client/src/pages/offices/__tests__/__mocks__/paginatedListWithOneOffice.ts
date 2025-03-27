import OfficeModel from "@shared/models/offices/officeModel";
import PaginatedList from "@shared/models/paginatedList";

export const paginatedListWithOneOffice: PaginatedList<OfficeModel> = {
  hasNextPage: true,
  hasPreviousPage: true,
  items: [
    {
      id: "1",
      address: "Office 1 address",
      registryPhoneNumber: "1234567890",
      isActive: true,
    },
  ],
  pageIndex: 2,
  totalPages: 3,
};
