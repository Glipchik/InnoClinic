import { createSlice } from '@reduxjs/toolkit';

interface EditOfficeState {
  loading: boolean;
  error: string | null;
}

const initialState : EditOfficeState = {
  loading: false,
  error: null,
};

const EditOfficeSlice = createSlice({
  name: 'EditOffice',
  initialState,
  reducers: {
    editOfficeRequest: state => {
      state.loading = true;
      state.error = null;
    },
    editOfficeSuccess: (state) => {
      state.loading = false;
    },
    editOfficeFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { editOfficeRequest, editOfficeSuccess, editOfficeFailure } = EditOfficeSlice.actions;
export default EditOfficeSlice.reducer;