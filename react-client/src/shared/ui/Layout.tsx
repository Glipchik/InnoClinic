import { Outlet } from "react-router-dom";
import { UserManagerContext } from '../contexts/UserManagerContext';
import Header from "./Header"
import Footer from "./Footer"
import { useDispatch } from "react-redux";
import { useContext, useEffect } from "react";
import { authorized } from "../../store/slices/authSlice"

function Layout() {
  const dispatch = useDispatch()
  const userManager = useContext(UserManagerContext);

  useEffect(() => {
    if (userManager) {
      userManager.events.addUserLoaded(() => dispatch(authorized()))
    }
  }, [dispatch, userManager])

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