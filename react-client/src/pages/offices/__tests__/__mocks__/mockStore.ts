import { createOfficeSliceReducer } from "@features/create-office-form/store/create-office";
import { editOfficeSliceReducer } from "@features/edit-office-form/store/edit-office";
import { deleteOfficeSliceReducer } from "@features/offices-list/store/delete-office";
import { configureStore } from "@reduxjs/toolkit";
import OfficeModel from "@shared/models/offices/officeModel";
import PaginatedList from "@shared/models/paginatedList";
import { fetchOfficesSliceReducer } from "@shared/store/fetch-offices";

export const createMockStore = (fetchOfficesState: {
  loading: boolean;
  data?: PaginatedList<OfficeModel>;
  error?: string;
}) => {
  return configureStore({
    reducer: {
      fetchOffices: fetchOfficesSliceReducer,
      deleteOffice: deleteOfficeSliceReducer,
      editOffice: editOfficeSliceReducer,
      createOffice: createOfficeSliceReducer,
    },
    preloadedState: {
      fetchOffices: fetchOfficesState,
      createOffice: { loading: false },
      deleteOffice: { loading: false },
      editOffice: { loading: false },
    },
  });
};
