import { createSlice } from '@reduxjs/toolkit';

interface CreateServiceCategoryState {
  loading: boolean;
  error: string | null;
}

const initialState : CreateServiceCategoryState = {
  loading: false,
  error: null,
};

const createServiceCategorySlice = createSlice({
  name: 'serviceCategories',
  initialState,
  reducers: {
    createServiceCategoryRequest: state => {
      state.loading = true;
      state.error = null;
    },
    createServiceCategorySuccess: (state) => {
      state.loading = false;
    },
    createServiceCategoryFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { createServiceCategoryRequest, createServiceCategorySuccess, createServiceCategoryFailure } = createServiceCategorySlice.actions;
export default createServiceCategorySlice.reducer;