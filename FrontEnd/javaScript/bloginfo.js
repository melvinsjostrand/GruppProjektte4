import {
	verify,
	logInOrLogOut,
	createPanel
} from "./verify.js";
let main;
let json = [];
let jsonComment = [];
let blog;
let article;
let div;
function init() {
	getVerify();
	main = document.getElementsByTagName("main")[0];
	div = document.getElementsByTagName("div")[1];
	console.log(div);
	blogPost();
	comments();
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
	createPanel(role)
}

async function blogPost() {
	const queryString = window.location.search;
	const urlParams = new URLSearchParams(queryString);
	blog = Number(urlParams.get("Id"));
	let path = "https://localhost:7063/Blog/" + blog;
	console.log(path);
	json = await getJson(path);
	console.log(json);
	for(blog of json){
		createBlog(blog);
	}
}

function createBlog(blog){
	article = document.createElement("article");
	let blogText = createHTMLElement("p", blog.text);
	let title = createHTMLElement("h3", blog.title);
	let timestamp = createHTMLElement("p", blog.time);
	div.appendChild(article);
	createfigure(blog);
	article.appendChild(blogText)
	article.appendChild(timestamp);
}

function createfigure(blog) {
	let figure = createHTMLElement("figure");
	let username = createHTMLElement("h2", blog.username);
	let img = createHTMLElement("img", null, {
		src: blog.img,
		alt: blog.name
	});
	figure.appendChild(img);
	figure.appendChild(username);
	article.appendChild(figure);

}


function createHTMLElement(tag, text = null, attributes = {}) {
	let element = document.createElement(tag);

	if (text !== null) {
		element.innerHTML = text;
	}

	for (let key in attributes) {
		element.setAttribute(key, attributes[key]);
	}

	return element;
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

async function getJson(path){
	let response = await fetch(path, {
		headers: {
			Authorization: localStorage.getItem("GUID"),
		},
	});
	let jsonData = await response.json();
	console.log(jsonData);
	return jsonData;
}