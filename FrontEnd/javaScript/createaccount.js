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
	let address = form.elements.address.value;
	error = document.getElementById("error");
	let json = {
		"username": username,
		"address":address
	}
	let status = await postFetch(json)
	if (status == 201) {
		location.href = "login.html";
	} else if (status == 500) {
		console.log(`Error: ${status}`);
		error.innerHTML = "gick inte ladda upp på grund av backend";
	} else {
		error.innerHTML = `fel användarnamn eller email`;
	}
}

async function postFetch(json) {
	let path = "https://localhost:7063/User";
	console.log(localStorage.getItem("GUID"));
	let Mail = form.elements.mail.value;
	let password = form.elements.password.value;
    console.log(Mail);
    console.log(password);
	const response = await fetch(path, {
		method: "POST",
		mode: "cors",
		headers: {
			"Content-type": "application/json",
			"Authorization": "Basic: " + btoa(Mail+":"+password)
		},
		body: JSON.stringify(json)
	})
	return response.status;
}