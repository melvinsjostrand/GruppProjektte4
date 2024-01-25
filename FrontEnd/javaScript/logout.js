import {
	verify,
	logInOrLogOut
} from "./verify.js";

function init() {
	getVerify();
	let guid = localStorage.getItem("GUID");
	console.log(guid);

	localStorage.removeItem("GUID");

	console.log(localStorage.getItem("GUID"));

	location.href = "index.html";
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
}