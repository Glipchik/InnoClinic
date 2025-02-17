import { createSlice } from '@reduxjs/toolkit';

interface DeleteDoctorState {
  loading: boolean;
  error: string | null;
}

const initialState : DeleteDoctorState = {
  loading: false,
  error: null,
};

const DeleteDoctorSlice = createSlice({
  name: 'DeleteDoctor',
  initialState,
  reducers: {
    deleteDoctorRequest: state => {
      state.loading = true;
      state.error = null;
    },
    deleteDoctorSuccess: (state) => {
      state.loading = false;
    },
    deleteDoctorFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { deleteDoctorRequest, deleteDoctorSuccess, deleteDoctorFailure } = DeleteDoctorSlice.actions;
export default DeleteDoctorSlice.reducer;