import { createSlice } from '@reduxjs/toolkit';

interface EditSpecializationState {
  loading: boolean;
  error: string | null;
}

const initialState : EditSpecializationState = {
  loading: false,
  error: null,
};

const EditSpecializationSlice = createSlice({
  name: 'EditSpecialization',
  initialState,
  reducers: {
    editSpecializationRequest: state => {
      state.loading = true;
      state.error = null;
    },
    editSpecializationSuccess: (state) => {
      state.loading = false;
    },
    editSpecializationFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { editSpecializationRequest, editSpecializationSuccess, editSpecializationFailure } = EditSpecializationSlice.actions;
export default EditSpecializationSlice.reducer;