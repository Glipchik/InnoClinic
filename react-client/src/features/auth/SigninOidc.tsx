import { useContext, useEffect } from "react";
import { UserManagerContext } from '../../shared/contexts/UserManagerContext';
import { useNavigate } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { authorized } from "../../store/store";

function SigninOidc() {
  const userManager = useContext(UserManagerContext);
  const navigate = useNavigate();
  const dispatch = useDispatch();

  useEffect(() => {
    if (!userManager) return;

    async function signinAsync() {
      try {
        const urlParams = new URLSearchParams(window.location.search);
        if (!urlParams.has("state")) {
          console.warn("No state found");
          return;
        }

        await userManager!.signinRedirectCallback();
        dispatch(authorized())
        console.log("dispatched authorize")
      } catch (error) {
        console.error("Error while login:", error);
      }
      navigate("/");
    }

    signinAsync();
  }, [userManager, navigate, dispatch]);

  return null;
}

export { SigninOidc };