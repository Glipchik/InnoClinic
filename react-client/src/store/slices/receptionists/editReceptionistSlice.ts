import { createSlice } from '@reduxjs/toolkit';

interface EditReceptionistState {
  loading: boolean;
  error: string | null;
}

const initialState : EditReceptionistState = {
  loading: false,
  error: null,
};

const EditReceptionistSlice = createSlice({
  name: 'EditReceptionist',
  initialState,
  reducers: {
    editReceptionistRequest: state => {
      state.loading = true;
      state.error = null;
    },
    editReceptionistSuccess: (state) => {
      state.loading = false;
    },
    editReceptionistFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { editReceptionistRequest, editReceptionistSuccess, editReceptionistFailure } = EditReceptionistSlice.actions;
export default EditReceptionistSlice.reducer;