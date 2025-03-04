import { Outlet } from "react-router-dom";
import { useContext, useEffect } from "react";
import { UserManagerContext } from "@shared/contexts/UserManagerContext";
import Footer from "../footer";
import Header from "../header";

const Layout = () => {
  const userManager = useContext(UserManagerContext);

  useEffect(() => {
    if (userManager) {
      userManager.events.addUserLoaded((user) => {
        localStorage.setItem("token", user.access_token)
      })
      userManager.events.addUserUnloaded(() => {
        localStorage.removeItem("token")
      })
      userManager.events.addAccessTokenExpired(() => {
        userManager.signinSilent()
      })
    }
  }, [userManager])

  return (
    <div>
      <Header />
      <main>
        <Outlet />
      </main>
      <Footer />
    </div>
  );
}

export default Layout;