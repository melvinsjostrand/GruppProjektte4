import {
	verify,
	logInOrLogOut,
	createPanel
} from "./verify.js";
let form;
let error;

function init() {
	getVerify();
	form = document.querySelector("form");
	form.addEventListener("submit", event => {
		getData();
		event.preventDefault();
	})
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
	createPanel(role);
}

async function getData() {
	let username = form.elements.username.value;
	let mail = form.elements.mail.value;
	let address = form.elements.address.value;
	let password = form.elements.password.value;
	error = document.getElementById("error");
	let header = "Basic: " + btoa(mail+":"+password);
	let json = {
		"username": username,
		"mail":mail,
		"address":address
	}
	let status = await postFetch(json, header)
	if (status == 201) {
		location.href = "login.html";
	} else if (status == 500) {
		console.log(`Error: ${status}`);
		error.innerHTML = "gick inte ladda upp på grund av backend";
	} else {
		error.innerHTML = `fel användarnamn eller email`;
	}
}

async function postFetch(json , header) {
	let path = "https://localhost:7063/User";
	const response = await fetch(path, {
		method: "POST",
		mode: "cors",
		headers: {
			"Content-type": "application/json",
			"Authorization": header
		},
		body: JSON.stringify(json)
	})
	console.log(json);
	return response.status;
}