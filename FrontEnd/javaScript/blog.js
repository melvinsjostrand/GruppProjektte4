import {
	verify,
	logInOrLogOut,
	seeBlogButtons,
	createPanel
} from "./verify.js";

let main;
let div;
let blogs = [];
let article;
let blog;
function init() {
	const queryString = window.location.search;
	const urlParams = new URLSearchParams(queryString);
	blog = Number(urlParams.get("Id"));
	console.log(blog);

	main = document.getElementsByTagName("main")[0];
	div = document.getElementsByTagName("div")[1];
	createblogs();
	getVerify();
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
	seeBlogButtons(role);
	createPanel(role);
}

async function createblogs() {
	let path = "https://localhost:7063/Blog/AllBlog";
	blogs = await getblog(path);
	console.log(blogs);

	blogs.forEach(blog => {
		createArticle(blog);
	});
}

function createArticle(blog) {
	article = createHTMLElement("article");
	createfigure(blog);
	let title = createHTMLElement("h2", ` ${blog.title}`);
	let timestamp = createHTMLElement("p", `Blogg gjord ${blog.time}`);
	article.addEventListener("click", event => {
		console.log("blog Id", blog.id);
		location.href = "bloginfo.html?Id=" + blog.id;
	})
	article.appendChild(title);
	article.appendChild(timestamp);
	div.appendChild(article);
}

function createfigure(blog) {
	let figure = createHTMLElement("figure");
	let username = createHTMLElement("figcaption", blog.username);
	let img = createHTMLElement("img", null, {
		src: blog.img,
		alt: blog.username
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

async function getblog(path) {
	let response = await fetch(path, {
		headers: {
			"Authorization": localStorage.getItem("GUID")
		}
	});
	if (response.status !== 200) {
		return "error";
	}
	let json = await response.json();
	return json;
}