import { useContext, useEffect, useState } from "react";
import { UserManagerContext } from '../../contexts/UserManagerContext';
import { User } from 'oidc-client';
import { Link } from "react-router-dom";
import Button from "../controls/Button";

const Header = () => {
  const userManager = useContext(UserManagerContext);
  const [user, setUser] = useState<User | null>(null);
  useEffect(() => {
    if (userManager) {
      async function fetchUser() {
        const user = await userManager!.getUser();
        setUser(user);
      }
      fetchUser();
    }
  }, [userManager]);

  return (
    <header className="w-full inline-flex py-7 px-3 items-center bg-gray-100 text-black">
      <div className="w-[25%] h-full">
        <h1 className="text-5xl font-bold">InnoClinic</h1>
      </div>
      <div className="w-[45%] h-full">

      </div>
      <div className="w-[30%] h-full">
        <div className="flex w-full h-full items items-center space-x-3 justify-end">
          {user ? (
            <>
              <div className="flex-col h-full space-y-0">
                <p className="text-xl">
                  {user.profile.email}
                </p>
                <p className="text-sm text-gray-800">
                  {user.profile.role}
                </p>
              </div>
              <li className="list-none"> <Link to="/logout"> <Button className="bg-red-600 hover:bg-red-700"> Logout </Button> </Link> </li>
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