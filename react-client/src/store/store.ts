import { createSlice, configureStore } from '@reduxjs/toolkit';

const authSlice = createSlice({
    name: 'auth',
    initialState: {
      isUserAuthorized: false
    },
    reducers: {
      authorized: state => {
        state.isUserAuthorized = true
      },
      logout: state => {
        state.isUserAuthorized = false
      }
    }
  })

const store = configureStore(authSlice);

export const { authorized, logout } = authSlice.actions;
export default store;