import { createSlice } from '@reduxjs/toolkit';

interface DeleteServiceCategoryState {
  loading: boolean;
  error: string | null;
}

const initialState : DeleteServiceCategoryState = {
  loading: false,
  error: null,
};

const deleteServiceCategorySlice = createSlice({
  name: 'serviceCategories',
  initialState,
  reducers: {
    deleteServiceCategoryRequest: state => {
      state.loading = true;
      state.error = null;
    },
    deleteServiceCategorySuccess: (state) => {
      state.loading = false;
    },
    deleteServiceCategoryFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { deleteServiceCategoryRequest, deleteServiceCategorySuccess, deleteServiceCategoryFailure } = deleteServiceCategorySlice.actions;
export default deleteServiceCategorySlice.reducer;