import { User } from "oidc-client";

export const mockUser = new User({
  access_token: "fake-token",
  id_token: "fake-id-token",
  session_state: "fake-session-state",
  refresh_token: "fake-refresh-token",
  token_type: "Bearer",
  scope: "openid profile email roles api_profile api_email api_roles",
  profile: {
    role: "Receptionist",
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
