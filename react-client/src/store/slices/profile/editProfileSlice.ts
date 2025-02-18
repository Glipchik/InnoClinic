import { createSlice } from "@reduxjs/toolkit";

interface EditProfileState {
  loading: boolean;
  error: string | null;
}

const initialState : EditProfileState = {
  loading: false,
  error: null,
};

const EditProfileSlice = createSlice({
  name: 'EditProfile',
  initialState,
  reducers: {
    editProfileRequest: state => {
      state.loading = true;
      state.error = null;
    },
    editProfileSuccess: (state) => {
      state.loading = false;
    },
    editProfileFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
});

export const { editProfileRequest, editProfileSuccess, editProfileFailure } = EditProfileSlice.actions;
export default EditProfileSlice.reducer;