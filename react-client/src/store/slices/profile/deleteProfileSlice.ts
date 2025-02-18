import { createSlice } from "@reduxjs/toolkit";

interface DeleteProfileState {
  loading: boolean;
  error: string | null;
}

const initialState : DeleteProfileState = {
  loading: false,
  error: null,
};

const DeleteProfileSlice = createSlice({
  name: 'DeleteProfile',
  initialState,
  reducers: {
    deleteProfileRequest: state => {
      state.loading = true;
      state.error = null;
    },
    deleteProfileSuccess: (state) => {
      state.loading = false;
    },
    deleteProfileFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { deleteProfileRequest, deleteProfileSuccess, deleteProfileFailure } = DeleteProfileSlice.actions;
export default DeleteProfileSlice.reducer;