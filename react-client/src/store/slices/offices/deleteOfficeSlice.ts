import { createSlice } from '@reduxjs/toolkit';

interface DeleteOfficeState {
  loading: boolean;
  error: string | null;
}

const initialState : DeleteOfficeState = {
  loading: false,
  error: null,
};

const DeleteOfficeSlice = createSlice({
  name: 'DeleteOffice',
  initialState,
  reducers: {
    deleteOfficeRequest: state => {
      state.loading = true;
      state.error = null;
    },
    deleteOfficeSuccess: (state) => {
      state.loading = false;
    },
    deleteOfficeFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { deleteOfficeRequest, deleteOfficeSuccess, deleteOfficeFailure } = DeleteOfficeSlice.actions;
export default DeleteOfficeSlice.reducer;