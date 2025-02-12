import { createSlice } from '@reduxjs/toolkit';
import ServiceCategory from '../../../entities/serviceCategory';
import PaginatedList from '../../../models/paginatedList';

interface FetchServiceCategoriesState {
  loading: boolean;
  error: string | null;
  data: ServiceCategory | ServiceCategory[] | PaginatedList<ServiceCategory> | null
}

const initialState : FetchServiceCategoriesState = {
  loading: false,
  error: null,
  data: null
};

const fetchServiceCategoriesSlice = createSlice({
  name: 'serviceCategories',
  initialState,
  reducers: {
    fetchServiceCategoriesRequest: state => {
      state.loading = true;
      state.error = null;
    },
    fetchServiceCategoriesSuccess: (state, action) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchServiceCategoriesFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchServiceCategoriesRequest, fetchServiceCategoriesSuccess, fetchServiceCategoriesFailure } = fetchServiceCategoriesSlice.actions;
export default fetchServiceCategoriesSlice.reducer;