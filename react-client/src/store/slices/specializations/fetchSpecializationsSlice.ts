import { createSlice } from '@reduxjs/toolkit';
import Specialization from '../../../entities/specialization';
import PaginatedList from '../../../models/paginatedList';

interface FetchSpecializationsState {
  loading: boolean;
  error: string | null;
  data: Specialization | Specialization[] | PaginatedList<Specialization> | null 
}

const initialState : FetchSpecializationsState = {
  loading: false,
  error: null,
  data: null
};

const specializationsSlice = createSlice({
  name: 'FetchSpecializations',
  initialState,
  reducers: {
    fetchSpecializationsRequest: state => {
      state.loading = true;
      state.error = null;
    },
    fetchSpecializationsSuccess: (state, action) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchSpecializationsFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchSpecializationsRequest, fetchSpecializationsSuccess, fetchSpecializationsFailure } = specializationsSlice.actions;
export default specializationsSlice.reducer;