import {
	verify,
	logInOrLogOut,
	createPanel
} from "./verify.js";
let form;
let error;
let Json;
let srcdata;

function init() {
	form = document.querySelector("form");
	getVerify();
	form.addEventListener("submit", event => {
		letFromData();
		event.preventDefault();
	})
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
	createPanel(role);
}

async function letFromData() {
	let title = form.elements.title.value;
	let desc = form.elements.desc.value;

	console.log(srcdata);
	console.log(title);
	console.log(desc);
	Json = {
		"title": title,
		"Img": srcdata,
		"text": desc
	}
    console.log(Json);
	let status = await postFetch(Json);
    console.log("Status" + status);
}
const fileInput = document.getElementById("fileinput");

fileInput.addEventListener("change", e => {
	const file = fileInput.files[0];
	const reader = new FileReader();
    console.log(file);
	reader.addEventListener("load", () => {
		console.log(reader.result);
		srcdata = reader.result;
	});
	reader.readAsDataURL(file);
});

async function postFetch(json) {
	let path = "https://localhost:7063/Blog";
	const response = await fetch(path, {
		method: 'POST',
		mode: "cors",
		headers: {
			"Content-type": "application/json",
			"authorization": localStorage.getItem("GUID")
		},
		body: JSON.stringify(json)
	})
	return response.status;
}