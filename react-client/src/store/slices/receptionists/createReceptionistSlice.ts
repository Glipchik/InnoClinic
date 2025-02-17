import { createSlice } from '@reduxjs/toolkit';

interface CreateReceptionistState {
  loading: boolean;
  error: string | null;
}

const initialState : CreateReceptionistState = {
  loading: false,
  error: null,
};

const CreateReceptionistSlice = createSlice({
  name: 'CreateReceptionist',
  initialState,
  reducers: {
    createReceptionistRequest: state => {
      state.loading = true;
      state.error = null;
    },
    createReceptionistSuccess: (state) => {
      state.loading = false;
    },
    createReceptionistFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { createReceptionistRequest, createReceptionistSuccess, createReceptionistFailure } = CreateReceptionistSlice.actions;
export default CreateReceptionistSlice.reducer;