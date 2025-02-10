import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  isUserAuthorized: false
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    authorized: state => {
      state.isUserAuthorized = true;
    },
    logout: state => {
      state.isUserAuthorized = false;
    }
  }
});

export const { authorized, logout } = authSlice.actions;
export default authSlice.reducer;