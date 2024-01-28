import {
	verify,
	logInOrLogOut,
	panelControl,
	createPanel
} from "./verify.js";

function init() {
	getVerify();
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
	panelControl(role);
	createPanel(role);
}