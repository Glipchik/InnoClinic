import { createSlice } from '@reduxjs/toolkit';

interface DeleteSpecializationState {
  loading: boolean;
  error: string | null;
}

const initialState : DeleteSpecializationState = {
  loading: false,
  error: null,
};

const DeleteSpecializationSlice = createSlice({
  name: 'DeleteSpecialization',
  initialState,
  reducers: {
    deleteSpecializationRequest: state => {
      state.loading = true;
      state.error = null;
    },
    deleteSpecializationSuccess: (state) => {
      state.loading = false;
    },
    deleteSpecializationFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { deleteSpecializationRequest, deleteSpecializationSuccess, deleteSpecializationFailure } = DeleteSpecializationSlice.actions;
export default DeleteSpecializationSlice.reducer;