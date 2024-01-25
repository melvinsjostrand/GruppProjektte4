import {
	verify,
	logInOrLogOut,
	panelControl
} from "./verify.js";

function init() {
	getVerify();
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
	panelControl(role);
}