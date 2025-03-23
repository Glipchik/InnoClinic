import { createOfficeSliceReducer } from "@features/create-office-form/store/create-office";
import { editOfficeSliceReducer } from "@features/edit-office-form/store/edit-office";
import { deleteOfficeSliceReducer } from "@features/offices-list/store/delete-office";
import { configureStore } from "@reduxjs/toolkit";
import { fetchOfficesSliceReducer } from "@shared/store/fetch-offices";

export const createMockStore = (createOfficeState: {
  loading: boolean;
  error?: string;
  success?: boolean;
}) => {
  return configureStore({
    reducer: {
      fetchOffices: fetchOfficesSliceReducer,
      deleteOffice: deleteOfficeSliceReducer,
      editOffice: editOfficeSliceReducer,
      createOffice: createOfficeSliceReducer,
    },
    preloadedState: {
      fetchOffices: { loading: false },
      createOffice: createOfficeState,
      deleteOffice: { loading: false },
      editOffice: { loading: false },
    },
  });
};
