import { createSlice } from '@reduxjs/toolkit';

interface DeletePatientState {
  loading: boolean;
  error: string | null;
}

const initialState : DeletePatientState = {
  loading: false,
  error: null,
};

const DeletePatientSlice = createSlice({
  name: 'DeletePatient',
  initialState,
  reducers: {
    deletePatientRequest: state => {
      state.loading = true;
      state.error = null;
    },
    deletePatientSuccess: (state) => {
      state.loading = false;
    },
    deletePatientFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { deletePatientRequest, deletePatientSuccess, deletePatientFailure } = DeletePatientSlice.actions;
export default DeletePatientSlice.reducer;