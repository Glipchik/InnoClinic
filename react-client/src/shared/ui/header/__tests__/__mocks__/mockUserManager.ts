import { UserManager } from "oidc-client";
import { mockUser } from "./mockUser";

export const mockUserManager = {
  getUser: jest.fn().mockResolvedValue(mockUser),
  signinRedirect: jest.fn(),
  signoutRedirect: jest.fn(),
  events: {
    addUserLoaded: jest.fn(),
    removeUserLoaded: jest.fn(),
    addUserUnloaded: jest.fn(),
    removeUserUnloaded: jest.fn(),
  },
} as unknown as UserManager;
