import {
	verify,
	logInOrLogOut,
	createPanel
} from "./verify.js";
let main;
let json = [];
let jsonComment = [];

function init() {
	getVerify();
	main = document.getElementsByTagName("main")[0];
	blogPost();
	comments();
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
	createPanel(role)
}

function blogPost() {
	let article
	article = document.createElement("article");
	let figure = document.createElement("figure");
	let img = document.createElement("img");
	let title = document.createElement("h2");
	let blogText = document.createElement("p");
	let username = document.createElement("h3");
	let timestamp = document.createElement("p");
	main.appendChild(article);
	article.appendChild(figure);
	figure.appendChild(title);
	figure.appendChild(username);
	figure.appendChild(img);
	title.innerHTML = "aWadadwa";
	figure.appendChild(blogText)
	figure.appendChild(timestamp);
}


function comments() {
	let div;
	div = document.createElement("div");
	main.appendChild(div);
	for (let i = 0; i < jsonComment.length; i++) {
		let usernameComment = document.createElement("h4");
		console.log(usernameComment);
		let comment = document.createElement("p");
		let timeStamp = document.createElement("small");
		div.appendChild(usernameComment);
		div.appendChild(comment);
		div.appendChild(timestamp);
	}

}

let path = "https://localhost:7063/Blog/" + Id;