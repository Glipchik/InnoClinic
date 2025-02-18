import { createSlice } from '@reduxjs/toolkit';
import Receptionist from '../../../entities/receptionist';
import PaginatedList from '../../../models/paginatedList';
import ReceptionistModel from '../../../models/receptionists/ReceptionistModel';

interface FetchReceptionistsState {
  loading: boolean;
  error: string | null;
  data: Receptionist | Receptionist[] | null | PaginatedList<ReceptionistModel>
}

const initialState : FetchReceptionistsState = {
  loading: false,
  error: null,
  data: null
};

const FetchReceptionistsSlice = createSlice({
  name: 'FetchReceptionists',
  initialState,
  reducers: {
    fetchReceptionistsRequest: state => {
      state.loading = true;
      state.error = null;
    },
    fetchReceptionistsSuccess: (state, action) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchReceptionistsFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchReceptionistsRequest, fetchReceptionistsSuccess, fetchReceptionistsFailure } = FetchReceptionistsSlice.actions;
export default FetchReceptionistsSlice.reducer;