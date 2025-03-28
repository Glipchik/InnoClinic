import { MemoryRouter, Route, Routes } from "react-router-dom";
import Header from "@shared/ui/header";
import store from "@app/store";
import { UserManagerContext } from "@shared/contexts/UserManagerContext";
import { Provider } from "react-redux";
import { UserManager, WebStorageStateStore } from "oidc-client";
import { mockUserManager } from "./__mocks__";
import { render, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import "@testing-library/jest-dom";

const config = {
  authority: "",
  client_id: "",
  redirect_uri: "",
  response_type: "",
  scope: "",
  post_logout_redirect_uri: "",
  userStore: new WebStorageStateStore({ store: window.localStorage }),
  stateStore: new WebStorageStateStore({ store: window.sessionStorage }),
  loadUserInfo: true,
  automaticSilentRenew: true,
  silent_redirect_uri: "",
  monitorSession: false,
};

const userManager = new UserManager(config);

const renderHeader = (userManager: UserManager) =>
  render(
    <Provider store={store}>
      <UserManagerContext.Provider value={userManager}>
        <MemoryRouter>
          <Routes>
            <Route
              path="/login"
              element={<h1 data-testid="login-page">Login</h1>}
            />
            <Route
              path="/logout"
              element={<h1 data-testid="logout-page">Logout</h1>}
            />
            <Route path="/" element={<Header />} />
          </Routes>
        </MemoryRouter>
      </UserManagerContext.Provider>
    </Provider>
  );

describe("Header", () => {
  it("renders required components when unauthorized", async () => {
    const { getByTestId, queryByTestId } = renderHeader(userManager);

    await waitFor(() => {
      expect(queryByTestId("email")).not.toBeInTheDocument();
      expect(queryByTestId("logout-button")).not.toBeInTheDocument();
      expect(queryByTestId("logo")).toBeInTheDocument();
    });

    const signInUpButton = getByTestId("sign-in-up-button");
    expect(signInUpButton).toBeInTheDocument();
    await userEvent.click(signInUpButton);

    await waitFor(() => {
      expect(queryByTestId("login-page")).toBeInTheDocument();
    });
  });

  it("renders required components when authorized", async () => {
    const { getByTestId, queryByTestId, queryByText } =
      renderHeader(mockUserManager);

    await waitFor(() => {
      const emailComponent = queryByTestId("email");
      expect(emailComponent).toBeInTheDocument();
      expect(emailComponent).toHaveTextContent("test@example.com");
      expect(queryByText("Receptionist")).toBeInTheDocument();
      expect(queryByTestId("logout-button")).toBeInTheDocument();
      expect(queryByTestId("logo")).toBeInTheDocument();
    });

    const logoutButton = getByTestId("logout-button");
    expect(logoutButton).toBeInTheDocument();
    await userEvent.click(logoutButton);

    await waitFor(() => {
      expect(queryByTestId("logout-page")).toBeInTheDocument();
    });
  });

  it("matches snapshot", () => {
    const { asFragment } = renderHeader(mockUserManager);
    expect(asFragment()).toMatchSnapshot();
  });
});
