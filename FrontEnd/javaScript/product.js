import {
	verify,
	logInOrLogOut,
	cart
} from "./verify.js";

let main;
let json = [];
let div;
let article;

function init() {
	getVerify();
	div = document.getElementsByTagName("div")[2];
	main = document.querySelector("main");
	createProducts();
}

window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
	cart(role);
}

async function createProducts() {
	let path = "https://localhost:7063/Product/AllProducts";
	json = await getProduct(path);
	console.log(json);

	json.forEach(product => {
		createArticle(product);
	});
}

function createArticle(product) {
	article = createHTMLElement("article");
	console.log(product.name);
	createfigure(product);
	let category = createHTMLElement("h3", `Category: ${product.category}`);
	let price = createHTMLElement("p", `Priset är ${product.price}kr`);
	let desc = createHTMLElement("p", product.description);
	let inStock = createHTMLElement("p", `i lager: ${product.stock}st`);
	let feeding = createHTMLElement("p", product.feeding);
	let rating = createHTMLElement("p", `⭐ ${product.rating}`);
	let articlenumber = createHTMLElement("p", `artikelnummer ${product.id}`);
	let button = createHTMLElement("button", "KÖP", {
		id: product.id
	});
	button.addEventListener("click", event => {
		console.log("product Id", product.id);
	})
	article.appendChild(category);
	article.appendChild(price);
	article.appendChild(desc);
	article.appendChild(rating);
	article.appendChild(articlenumber);
	article.appendChild(feeding);
	article.appendChild(inStock);
	article.appendChild(button);
	div.appendChild(article);
}

function createfigure(product) {
	let figure = createHTMLElement("figure");
	let name = createHTMLElement("h2", product.name);
	let img = createHTMLElement("img", null, {
		src: product.img,
		alt: product.name
	});
	figure.appendChild(img);
	figure.appendChild(name);
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

async function getProduct(path) {
	console.log(localStorage.getItem("GUID"));
	let response = await fetch(path, {
		headers: {
			Authorization: localStorage.getItem("GUID"),
		},
	});
	if (response.status !== 200) {
		return "error";
	}
	let jsonData = await response.json();
	return jsonData;
}