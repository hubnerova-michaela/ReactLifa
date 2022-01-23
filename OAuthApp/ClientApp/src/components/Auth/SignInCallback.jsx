import { useNavigate } from "react-router-dom";
import { useAuthContext } from "../../providers/AuthProvider";

export const SignInCallback = props => {
    const [{ userManager }] = useAuthContext();
    let navigate = useNavigate();
    userManager.signinRedirectCallback();
    navigate("/");
    return null;
}

export default SignInCallback;