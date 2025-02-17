import { createSlice } from '@reduxjs/toolkit';

interface EditDoctorState {
  loading: boolean;
  error: string | null;
}

const initialState : EditDoctorState = {
  loading: false,
  error: null,
};

const EditDoctorSlice = createSlice({
  name: 'EditDoctor',
  initialState,
  reducers: {
    editDoctorRequest: state => {
      state.loading = true;
      state.error = null;
    },
    editDoctorSuccess: (state) => {
      state.loading = false;
    },
    editDoctorFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { editDoctorRequest, editDoctorSuccess, editDoctorFailure } = EditDoctorSlice.actions;
export default EditDoctorSlice.reducer;