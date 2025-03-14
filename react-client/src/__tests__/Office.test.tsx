import store from "@app/store";
import { OfficesPage } from "@pages/offices";
import { UserManagerContext } from "@shared/contexts/UserManagerContext";
import { render, screen } from "@testing-library/react";
import { WebStorageStateStore, UserManager } from "oidc-client";
import { Provider } from "react-redux";
import { MemoryRouter } from "react-router-dom";
import { describe, it, expect, vi } from "vitest";
import officesApi from "@shared/api/offices";
import PaginatedList from "@shared/models/paginatedList";
import OfficeModel from "@shared/models/offices/officeModel";

const emptyPaginatedList: PaginatedList<OfficeModel> = {
  hasNextPage: false,
  hasPreviousPage: false,
  items: [
    {
      id: "",
      address: "Office 1 address",
      registryPhoneNumber: "1234567890",
      isActive: true,
    }
  ],
  pageIndex: 1,
  totalPages: 1,
};

officesApi.getAll = vi.fn().mockResolvedValue(emptyPaginatedList);

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

const userManager = new UserManager(config);

const renderOffices = (userManager: UserManager) =>
  render(
    <Provider store={store}>
      <UserManagerContext.Provider value={userManager}>
        <MemoryRouter>
          <OfficesPage />
        </MemoryRouter>
      </UserManagerContext.Provider>
    </Provider>
  );

describe("Offices", () => {
  it("does not render offices", async () => {
    document.cookie = "";
    renderOffices(userManager);

    expect(screen.getByText("Offices")).toBeInTheDocument();

    const listItems = await screen.findAllByTestId("office-card");
    console.log(listItems)
    expect(listItems.length).toBe(0);
  });

  // it('renders user profile when authorized', async () => {
  //   renderOffices(mockUserManager)
  //   expect(await screen.findByText('test@example.com')).toBeInTheDocument()
  // })

  // it('renders logout button when authorized', async () => {
  //   renderOffices(mockUserManager)
  //   expect(await screen.findByText('Logout')).toBeInTheDocument()
  // })
});
