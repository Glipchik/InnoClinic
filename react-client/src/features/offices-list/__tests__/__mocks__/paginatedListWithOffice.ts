import OfficeModel from "@shared/models/offices/officeModel";
import PaginatedList from "@shared/models/paginatedList";

export const paginatedListWithOffice: PaginatedList<OfficeModel> = {
  hasNextPage: false,
  hasPreviousPage: false,
  items: [
    {
      id: "1",
      address: "Office 1 address",
      registryPhoneNumber: "1234567890",
      isActive: true,
    },
  ],
  pageIndex: 1,
  totalPages: 1,
};
