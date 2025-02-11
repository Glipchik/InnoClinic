import { createSlice } from '@reduxjs/toolkit';
import Specialization from '../../entities/specialization';
import PaginatedList from '../../models/paginatedList';

interface SpecializationsState {
  loading: boolean;
  error: string | null;
  specializationsData: Specialization | Specialization[] | null 
  paginatedSpecializationsData: PaginatedList<Specialization> | null,
}

const initialState : SpecializationsState = {
  loading: false,
  error: null,
  specializationsData: null,
  paginatedSpecializationsData: null,
};

const specializationsSlice = createSlice({
  name: 'specializations',
  initialState,
  reducers: {
    fetchSpecializationsDataRequest: state => {
      state.loading = true;
      state.error = null;
    },
    fetchSpecializationsDataSuccess: (state, action) => {
      state.loading = false;
      state.specializationsData = action.payload;
    },
    fetchPaginatedSpecializationsDataSuccess: (state, action) => {
      state.loading = false;
      state.paginatedSpecializationsData = action.payload;
    },
    fetchSpecializationsDataFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchSpecializationsDataRequest, fetchSpecializationsDataSuccess, fetchPaginatedSpecializationsDataSuccess, fetchSpecializationsDataFailure } = specializationsSlice.actions;
export default specializationsSlice.reducer;