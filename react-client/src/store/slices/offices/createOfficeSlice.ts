import { createSlice } from '@reduxjs/toolkit';

interface CreateOfficeState {
  loading: boolean;
  error: string | null;
}

const initialState : CreateOfficeState = {
  loading: false,
  error: null,
};

const CreateOfficeSlice = createSlice({
  name: 'CreateOffice',
  initialState,
  reducers: {
    createOfficeRequest: state => {
      state.loading = true;
      state.error = null;
    },
    createOfficeSuccess: (state) => {
      state.loading = false;
    },
    createOfficeFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { createOfficeRequest, createOfficeSuccess, createOfficeFailure } = CreateOfficeSlice.actions;
export default CreateOfficeSlice.reducer;