import { createSlice } from "@reduxjs/toolkit";
import DoctorModel from "../../../models/doctors/DoctorModel";
import PatientModel from "../../../models/patients/PatientModel";
import ReceptionistModel from "../../../models/receptionists/ReceptionistModel";

interface FetchProfileState {
  loading: boolean;
  error: string | null;
  data: PatientModel | DoctorModel | ReceptionistModel | null
}

const initialState : FetchProfileState = {
  loading: false,
  error: null,
  data: null
};

const FetchProfileSlice = createSlice({
  name: 'FetchProfile',
  initialState,
  reducers: {
    fetchProfileRequest: state => {
      state.loading = true;
      state.error = null;
    },
    fetchProfileSuccess: (state, action) => {
      state.loading = false;
      state.data = action.payload;
    },
    fetchProfileFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { fetchProfileRequest, fetchProfileSuccess, fetchProfileFailure } = FetchProfileSlice.actions;
export default FetchProfileSlice.reducer;