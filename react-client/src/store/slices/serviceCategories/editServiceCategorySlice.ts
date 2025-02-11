import { createSlice } from '@reduxjs/toolkit';

interface EditServiceCategoryState {
  loading: boolean;
  error: string | null;
}

const initialState : EditServiceCategoryState = {
  loading: false,
  error: null,
};

const editServiceCategorySlice = createSlice({
  name: 'serviceCategories',
  initialState,
  reducers: {
    editServiceCategoryRequest: state => {
      state.loading = true;
      state.error = null;
    },
    editServiceCategorySuccess: (state) => {
      state.loading = false;
    },
    editServiceCategoryFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { editServiceCategoryRequest, editServiceCategorySuccess, editServiceCategoryFailure } = editServiceCategorySlice.actions;
export default editServiceCategorySlice.reducer;