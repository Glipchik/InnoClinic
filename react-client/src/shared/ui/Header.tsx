import { useContext, useEffect, useState } from "react";
import { UserManagerContext } from '../contexts/UserManagerContext';
import { User } from 'oidc-client';
import { Link } from "react-router-dom";
import { useSelector } from 'react-redux';
import Button from "./controls/Button";
import { RootState } from "../../store/store";
import { GETProfilePictureUrl } from "../api/profileApi";
import profile_pic from "../../assets/profile_pic.png"

function Header() {
  const userManager = useContext(UserManagerContext);
  const [user, setUser] = useState<User | null>(null);
  const [photoUrl, setPhotoUrl] = useState<string | null>(null)

  const { isUserAuthorized } = useSelector(
    (state: RootState) => state.auth
  );

  useEffect(() => {
    if (userManager) {
      async function fetchUser() {
        const user = await userManager!.getUser();
        setUser(user);
      }
      fetchUser();
    }
  }, [userManager, isUserAuthorized]);

  useEffect(() => {
    if (user) {
      async function fetchPhoto() {
        try { 
          if (user?.access_token && user?.profile.sub) {
            const response = await GETProfilePictureUrl(user?.profile.sub, user?.access_token);
            setPhotoUrl(response.data)        
          }
        } catch {
          setPhotoUrl(null)
        }
      }

      fetchPhoto();
    }
  }, [user])

  return (
    <header className="w-full inline-flex py-7 px-3 items-center bg-gray-100 text-black">
      <div className="w-[25%] h-full">
        <h1 className="text-5xl font-bold">InnoClinic</h1>
      </div>
      <div className="w-[45%] h-full">
        {user?.profile.role === "Receptionist" && (
          <ul className="flex flex-row space-x-4">
            <li className="list-none"> <Link to="/offices" className="text-xl"> Offices </Link> </li>
            <li className="list-none"> <Link to="/specializations" className="text-xl"> Specializations </Link> </li>
            <li className="list-none"> <Link to="/services" className="text-xl"> Services </Link> </li>
            <li className="list-none"> <Link to="/service-categories" className="text-xl"> Service Categories </Link> </li>
          </ul>
        )}
      </div>
      <div className="w-[30%] h-full">
        <div className="flex w-full h-full items items-center space-x-3 justify-end">
          {user ? (
            <>
              <div className="flex flex-row space-x-1">
                <img src={photoUrl ? photoUrl : profile_pic} className="w-16 h-16 rounded-full object-cover"/>
                <div className="flex-col h-full space-y-0">
                  <p className="text-xl">
                    {user.profile.email}
                  </p>
                  <p className="text-sm text-gray-800">
                    {user.profile.role}
                  </p>
                </div>
                <li className="list-none"> <Link to="/logout"> <Button className="bg-red-600 hover:bg-red-700"> Logout </Button> </Link> </li>
              </div>
            </>
          ) : (
            <>
              <li className="list-none"> <Link to="/login"> <Button> Login </Button> </Link> </li>
              <li className="list-none">  <Link to="/register"> <Button> Registration </Button> </Link> </li>
            </>
          )}
        </div>
      </div>
    </header>
  );
}

export default Header;