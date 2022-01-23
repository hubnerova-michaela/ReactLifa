import { useNavigate } from "react-router-dom";
import { useAuthContext } from "../../providers/AuthProvider";

const SignOutCallback = props => {
    const [{ userManager }] = useAuthContext();
    let navigate = useNavigate();
    userManager.signoutRedirectCallback();
    navigate("/");
    return null;
}

export default SignOutCallback;