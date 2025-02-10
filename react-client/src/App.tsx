import "./App.css";
import { UserManagerContext } from './shared/contexts/UserManagerContext';
import { RouterProvider } from "react-router-dom";
import { Log, UserManager, WebStorageStateStore } from 'oidc-client';
import router from "./routes/router";
import { store } from './store/store';
import { Provider } from 'react-redux';

Log.logger = console
Log.level = Log.INFO

const config = {
  authority: import.meta.env.VITE_AUTH_SERVER_BASE_URL,
  client_id: import.meta.env.VITE_AUTH_CLIENT_ID,
  redirect_uri: `${import.meta.env.VITE_BASE_URL}/signin-oidc`,
  response_type: "code",
  scope: "openid profile email roles api_profile api_email api_roles",
  post_logout_redirect_uri: `${import.meta.env.VITE_BASE_URL}/`,
  userStore: new WebStorageStateStore({ store: window.localStorage }),
  stateStore: new WebStorageStateStore({ store: window.sessionStorage }),
  loadUserInfo: true,
  automaticSilentRenew: true,
  silent_redirect_uri: `${import.meta.env.VITE_BASE_URL}/silent-renew`,
  monitorSession: false,
}

const userManager = new UserManager(config)

userManager.events.addUserLoaded((user) => {
  console.log("User loaded", user)
})

userManager.events.addSilentRenewError((error) => {
  console.error("Silent renew error", error)
})

userManager.events.addUserSignedOut(() => {
  console.log("User signed out")
})

function App() {
  return (
    <Provider store={store}>
      <UserManagerContext.Provider value={userManager}>
        <RouterProvider router={router} />
      </UserManagerContext.Provider>
    </Provider>
  );
}

export default App;