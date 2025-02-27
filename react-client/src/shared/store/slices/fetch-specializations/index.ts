import SpecializationModel from '@models/specializations/specializationModel';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface FetchSpecializationsState {
  loading: boolean;
  error?: string;
  data?: SpecializationModel[]
}

const initialState : FetchSpecializationsState = {
  loading: false,
  error: undefined,
  data: undefined
};

const fetchSpecializationsSlice = createSlice({
  name: 'FetchSpecializationsSlice',
  initialState,
  reducers: {
    fetchSpecializationsRequest: state => {
      state.loading = true;
    },
    fetchSpecializationsSuccess: (state, action: PayloadAction<SpecializationModel[]>) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchSpecializationsFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchSpecializationsRequest, fetchSpecializationsFailure, fetchSpecializationsSuccess } = fetchSpecializationsSlice.actions;
export const fetchSpecializationsSliceReducer = fetchSpecializationsSlice.reducer