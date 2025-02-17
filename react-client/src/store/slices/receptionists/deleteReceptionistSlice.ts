import { createSlice } from '@reduxjs/toolkit';

interface DeleteReceptionistState {
  loading: boolean;
  error: string | null;
}

const initialState : DeleteReceptionistState = {
  loading: false,
  error: null,
};

const DeleteReceptionistSlice = createSlice({
  name: 'DeleteReceptionist',
  initialState,
  reducers: {
    deleteReceptionistRequest: state => {
      state.loading = true;
      state.error = null;
    },
    deleteReceptionistSuccess: (state) => {
      state.loading = false;
    },
    deleteReceptionistFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { deleteReceptionistRequest, deleteReceptionistSuccess, deleteReceptionistFailure } = DeleteReceptionistSlice.actions;
export default DeleteReceptionistSlice.reducer;