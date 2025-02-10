import { createContext } from "react";
import { UserManager } from 'oidc-client';

export const UserManagerContext = createContext<UserManager | null>(null);