import {
	verify,
	logInOrLogOut
} from "./verify.js";
let form;
let select;
let content;
let feeding;
let Json;
let labelsAfterSelect;
let srcdata;


function init() {
	form = document.querySelector("form");
	content = form.elements.content;
	feeding = form.elements.feeding;
    labelsAfterSelect = form.querySelectorAll("label");
    console.log(labelsAfterSelect);
    
	select = form.elements.category;
    console.log(select.value);
    selectedValue();
	getVerify();
	form.addEventListener("submit", event => {
		letFromData();
		event.preventDefault();
	})
	select.addEventListener("change", event => {
		selectedValue();
	})
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
}

async function letFromData() {
	let name = form.elements.productname.value;
	let desc = form.elements.desc.value;
	let category = select.value;
	let price = form.elements.price.value;
	let stock = form.elements.stock.value;
	content = form.elements.content.value;
	feeding = form.elements.feeding.value;
	Json = {
		"price": price,
		"category": category,
		"name": name,
		"img": srcdata,
		"description": desc,
		"stock": stock,
		"content": content,
		"feeding": feeding
	}
	console.log(Json);
	let status = await postFetch(Json);
}
const fileInput = document.getElementById("fileinput");

fileInput.addEventListener("change", e => {
	const file = fileInput.files[0];
	const reader = new FileReader();

	reader.addEventListener("load", () => {
		console.log(reader.result);
		srcdata = reader.result;
	});
	reader.readAsDataURL(file);
});

function selectedValue() {
	if (select.value == "Foder") {
		content.style.display = "block";
		feeding.style.display = "block";
        labelsAfterSelect[4].style.display = "block";
        labelsAfterSelect[5].style.display = "block";
	} else {
		content.style.display = "none";
		feeding.style.display = "none";
        labelsAfterSelect[4].style.display = "none";
        labelsAfterSelect[5].style.display = "none";
	}
}

async function postFetch(json) {
	let path = "https://localhost:7063/product";
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