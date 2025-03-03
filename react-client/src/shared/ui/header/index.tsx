import { useContext, useEffect, useState } from "react";
import { User } from 'oidc-client';
import { UserManagerContext } from "@shared/contexts/UserManagerContext";
import Logo from "@widgets/logo";
import UserInfo from "@widgets/user-info";

const Header = () => {
  const userManager = useContext(UserManagerContext);
  const [user, setUser] = useState<User | null | undefined>();
  useEffect(() => {
    if (!userManager) return;

    const fetchUser = async () => {
      const currentUser = await userManager?.getUser();
      setUser(currentUser);
    }

    fetchUser();

    const onUserLoaded = (loadedUser: User) => setUser(loadedUser);
    const onUserUnloaded = () => setUser(null);

    userManager.events.addUserLoaded(onUserLoaded);
    userManager.events.addUserUnloaded(onUserUnloaded);

    return () => {
      userManager.events.removeUserLoaded(onUserLoaded);
      userManager.events.removeUserUnloaded(onUserUnloaded);
    };
  }, [userManager]);

  return (
    <header className="w-full inline-flex py-7 px-3 items-center bg-gray-100 text-black">
      <Logo />
      <div className="w-[45%] h-full">

      </div>
      <div className="w-[30%] h-full">
        <UserInfo user={user} /> 
      </div>
    </header>
  );
}

export default Header;