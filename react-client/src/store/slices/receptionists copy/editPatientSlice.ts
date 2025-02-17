import { createSlice } from '@reduxjs/toolkit';

interface EditPatientState {
  loading: boolean;
  error: string | null;
}

const initialState : EditPatientState = {
  loading: false,
  error: null,
};

const EditPatientSlice = createSlice({
  name: 'EditPatient',
  initialState,
  reducers: {
    editPatientRequest: state => {
      state.loading = true;
      state.error = null;
    },
    editPatientSuccess: (state) => {
      state.loading = false;
    },
    editPatientFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { editPatientRequest, editPatientSuccess, editPatientFailure } = EditPatientSlice.actions;
export default EditPatientSlice.reducer;