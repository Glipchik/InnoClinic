import { createSlice } from '@reduxjs/toolkit';

interface CreateSpecializationState {
  loading: boolean;
  error: string | null;
}

const initialState : CreateSpecializationState = {
  loading: false,
  error: null,
};

const CreateSpecializationSlice = createSlice({
  name: 'CreateSpecialization',
  initialState,
  reducers: {
    createSpecializationRequest: state => {
      state.loading = true;
      state.error = null;
    },
    createSpecializationSuccess: (state) => {
      state.loading = false;
    },
    createSpecializationFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { createSpecializationRequest, createSpecializationSuccess, createSpecializationFailure } = CreateSpecializationSlice.actions;
export default CreateSpecializationSlice.reducer;