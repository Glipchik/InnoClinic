import { createSlice } from '@reduxjs/toolkit';

interface CreateServiceState {
  loading: boolean;
  error: string | null;
}

const initialState : CreateServiceState = {
  loading: false,
  error: null
};

const CreateServiceSlice = createSlice({
  name: 'CreateService',
  initialState,
  reducers: {
    createServiceRequest: state => {
      state.loading = true;
      state.error = null;
    },
    createServiceSuccess: (state) => {
      state.loading = false;
    },
    createServiceFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { createServiceRequest, createServiceSuccess, createServiceFailure } = CreateServiceSlice.actions;
export default CreateServiceSlice.reducer;