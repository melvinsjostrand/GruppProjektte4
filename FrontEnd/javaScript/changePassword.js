import {
	verify,
	logInOrLogOut,
	createPanel
} from "./verify.js";

function init() {
	getVerify();
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
	createPanel(role);
}

function checkPassword() {
	let password1 = document.getElementById("password1").value;
	let password2 = document.getElementById("password2").value;
	if (password1 !== password2) {
		alert('Passwords do not match');
		return false;
	}
}