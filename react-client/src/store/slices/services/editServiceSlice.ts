import { createSlice } from '@reduxjs/toolkit';

interface EditServiceState {
  loading: boolean;
  error: string | null;
}

const initialState : EditServiceState = {
  loading: false,
  error: null
};

const EditServiceSlice = createSlice({
  name: 'EditService',
  initialState,
  reducers: {
    editServiceRequest: state => {
      state.loading = true;
      state.error = null;
    },
    editServiceSuccess: (state) => {
      state.loading = false;
    },
    editServiceFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { editServiceRequest, editServiceSuccess, editServiceFailure } = EditServiceSlice.actions;
export default EditServiceSlice.reducer;