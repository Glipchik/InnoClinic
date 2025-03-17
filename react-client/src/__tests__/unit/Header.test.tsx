import { render, screen } from "@testing-library/react";
import { describe, expect, it, vi } from "vitest";
import { MemoryRouter } from "react-router-dom";
import Header from "../../shared/ui/header";
import store from "@app/store";
import { UserManagerContext } from "@shared/contexts/UserManagerContext";
import { Provider } from "react-redux";
import { User, UserManager, WebStorageStateStore } from "oidc-client";

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
};

const mockUser = new User({
  access_token: "fake-token",
  id_token: "fake-id-token",
  session_state: "fake-session-state",
  refresh_token: "fake-refresh-token",
  token_type: "Bearer",
  scope: "openid profile email roles api_profile api_email api_roles",
  profile: {
    name: "Test User",
    email: "test@example.com",
    iss: "https://example.com",
    sub: "1234567890",
    aud: "your-client-id",
    exp: Math.floor(Date.now() / 1000) + 3600,
    iat: Math.floor(Date.now() / 1000),
  },
  expires_at: Math.floor(Date.now() / 1000) + 3600,
  state: {},
});

const userManager = new UserManager(config);

const renderHeader = (userManager: UserManager) =>
  render(
    <Provider store={store}>
      <UserManagerContext.Provider value={userManager}>
        <MemoryRouter>
          <Header />
        </MemoryRouter>
      </UserManagerContext.Provider>
    </Provider>
  );

const mockUserManager = {
  getUser: vi.fn().mockResolvedValue(mockUser),
  signinRedirect: vi.fn(),
  signoutRedirect: vi.fn(),
  events: {
    addUserLoaded: vi.fn(),
    removeUserLoaded: vi.fn(),
    addUserUnloaded: vi.fn(),
    removeUserUnloaded: vi.fn(),
  },
} as unknown as UserManager;

describe("Header", () => {
  it("renders sign in/up button when unauthorized", () => {
    document.cookie = "";
    renderHeader(userManager);

    expect(screen.getByTestId("sign-in-up-button")).toBeInTheDocument();
    expect(screen.getByTestId("sign-in-up-button")).toBeEnabled();
  });

  it("renders user profile when authorized", async () => {
    renderHeader(mockUserManager);
    expect(await screen.findByTestId("email")).toHaveTextContent(
      "test@example.com"
    );
  });

  it("renders logout button when authorized", async () => {
    renderHeader(mockUserManager);
    expect(await screen.findByTestId("logout-button")).toBeInTheDocument();
  });
});
