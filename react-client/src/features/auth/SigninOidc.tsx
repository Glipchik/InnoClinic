import { useContext, useEffect } from "react"
import { UserManagerContext } from "../../shared/contexts/UserManagerContext"
import { useNavigate } from "react-router-dom"

function SigninOidc() {
  const userManager = useContext(UserManagerContext)
  const navigate = useNavigate()

  useEffect(() => {
    if (!userManager) return

    async function signinAsync() {
      try {
        const user = await userManager!.signinRedirectCallback()
        if (user) {
          console.log("User signed in successfully", user)
          navigate("/")
        } else {
          console.error("No user data received after sign-in")
          navigate("/login")
        }
      } catch (error) {
        console.error("Error during sign-in:", error)
      }
    }

    signinAsync()
  }, [userManager, navigate])

  return null
}

export { SigninOidc }