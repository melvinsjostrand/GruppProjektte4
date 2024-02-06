import {
	verify,
	logInOrLogOut,
	createPanel
} from "./verify.js";
let form;
let json = [];
function init() {
	form = document.querySelectorAll("form");
	getVerify();
	form.addEventListener("submit", event =>{
		change();
		event.preventDefault();
	})
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
	createPanel(role);
}


async function change(){
	let password = form.elements.password.value;
	json = {
		"password": password
	};
	let status = await changePassword(json);
}

async function changePassword(json){
	let path = "https://localhost:7063/User/Changepassword";
	const response = await fetch(path, {
		method: 'PUT',
		mode: "cors",
		headers:{
			"Content-type": "application/json",
            "Authorization": localStorage.getItem("GUID")
		},
		body: JSON.stringify(json),
	});
	// If the request is successful, no errors will be thrown and the promise resolves
	return response.status;
}