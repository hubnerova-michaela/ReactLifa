import { useAuthContext } from "../providers/AuthProvider";
import { Button } from 'reactstrap';
import { Link } from "react-router-dom";

const Home = () => {
	const [{ userManager, accessToken }] = useAuthContext();
	return (
		<div className="text-center">
			<h1>L I F△</h1>
			{accessToken
				? <button class="btn-primary" onClick={() => { userManager.signoutRedirect() }} >Sign Out</button>
				: <button class="btn-primary" onClick={() => { userManager.signinRedirect() }} >Sign In</button>
			}
		</div>
	);
}

export default Home; 

