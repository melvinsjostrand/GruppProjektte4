import {
	verify,
	logInOrLogOut,
} from "./verify.js";

let url = "https://localhost:7063/Product"; 
let main;
let json = [];
let div;
let article;

function init() {
	getVerify();
	div = document.getElementsByTagName("div")[1];
	main = document.querySelector("main");
	createProducts();

}

window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
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
	let articlenumber = createHTMLElement("p", `artikelnummer ${product.id}`);
	let button = createHTMLElement("button", "Ta bort", {
		id: product.id
	});
	button.addEventListener("click", event => {
		console.log("product Id", product.id);
        deleteProduct(product.id);
	})
	article.appendChild(category);
	article.appendChild(price);
	article.appendChild(desc);
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

async function deleteProduct(productId) {
    try {
        let confirmation = confirm("Are you sure you want to delete this product?");
        if (confirmation) {
            let deleteResponse = await deletefetch(productId);
            if (deleteResponse >= 200 && deleteResponse < 300) {
                alert("Product deleted successfully!");
                // You may want to update the UI here to reflect the deletion
            } else {
                alert("Error deleting product. Please try again.");
            }
        }
    } catch (error) {
        console.error("Error deleting product:", error);
        alert("Error deleting product. Please try again.");
    }
}

async function deletefetch(productId) {
    let deleteProduct = { id: productId };
    let response = await fetch(url, {
        method: 'DELETE',
        headers: {
            "Content-type": "application/json",
            "Authorization": localStorage.getItem("GUID")
        },
        body: JSON.stringify(deleteProduct)
    });
    return response.status;

}