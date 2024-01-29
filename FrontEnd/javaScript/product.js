import {
	verify,
	logInOrLogOut,
	cart,
	createPanel
} from "./verify.js";
let i
let names = ["Alla produkter","Djur","Foder","Annat"];
let category;
let main;
let json = [];
let div;
let article;
let button;
let divbutton;

function init() {
	getVerify();
	divbutton = document.getElementsByTagName("div")[1];
	div = document.getElementsByTagName("div")[2];
	main = document.querySelector("main");
	let buttons = document.getElementsByTagName("button");
	createButtons();
	createProducts();

	for (let i = 0; i < buttons.length; i++) {
		buttons[i].addEventListener("click", event => {
		  while (div.firstChild) {
			div.removeChild(div.firstChild);
		  }      
		  const buttonText = event.target.value; // Get the lowercase value of the clicked button
		  if (buttonText == "Alla produkter") {
			createProducts();
		  } else if(buttonText == "Djur"){
			category = names[1];
			console.log(category);
			createProductsCategory();
		  }else if(buttonText == "Foder"){
			category = names[2];
			console.log(category);
			createProductsCategory();
		}else if(buttonText == "Annat"){
			category = names[3];
			console.log(category);
			createProductsCategory();
		}
	});
}
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
	cart(role);
	createPanel(role);
}

async function createProducts() {
	let path = "https://localhost:7063/Product/AllProducts";
	json = await getProduct(path);
	console.log(json);

	json.forEach(product => {
		createArticle(product);
	});
}
async function createProductsCategory(){
	let url = "https://localhost:7063/Product/Category/" + category;
	json = await fetchData(url);
	console.log(json);
	json.forEach(product =>{
		createArticle(product);
	})
}

function createButtons(){
	for(i = 0; i < 4; i++){
		createItems();
	}
}

function createItems(){
	button = document.createElement("button");
	button.innerHTML = names[i];
	button.value = names[i];
	button.addEventListener("click", event => {
		console.log();
	})
	divbutton.appendChild(button);	
}
function createArticle(product) {
	article = createHTMLElement("article");
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
		postData(product.id);
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

async function fetchData(url){
	let response = await fetch(url, {
		headers: {
			Authorization: localStorage.getItem("GUID"),
		},
	});
	if (response.status !== 200) {
		return "error";
	}
	let jsonData = await response.json();
	console.log(jsonData);
	return jsonData;
}

async function postData(Id) {
    const post = "https://localhost:7063/Cart";
    console.log(post);
    try {
        const response = await fetch(post, {
            method: 'POST',
            mode: "cors",
            headers: {
                "Content-type": "application/json",
                "Authorization": localStorage.getItem("GUID")
            },
            body: JSON.stringify({ id: Id })
        });
        console.log(JSON.stringify({ id: Id }));
        return response.status;
    } catch (error) {
        throw new Error("Error in postData function: " + error.message);
    }
}