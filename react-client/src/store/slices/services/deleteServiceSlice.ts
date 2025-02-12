import { createSlice } from '@reduxjs/toolkit';

interface DeleteServiceState {
  loading: boolean;
  error: string | null;
}

const initialState : DeleteServiceState = {
  loading: false,
  error: null
};

const DeleteServiceSlice = createSlice({
  name: 'DeleteService',
  initialState,
  reducers: {
    deleteServiceRequest: state => {
      state.loading = true;
      state.error = null;
    },
    deleteServiceSuccess: (state) => {
      state.loading = false;
    },
    deleteServiceFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { deleteServiceRequest, deleteServiceSuccess, deleteServiceFailure } = DeleteServiceSlice.actions;
export default DeleteServiceSlice.reducer;