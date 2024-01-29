import {
	verify,
	logInOrLogOut,
	createPanel
} from "./verify.js";
let div;
let select;
let cart = [];
function init() {
	getVerify();
	div = document.getElementsByTagName("div")[1];
	getItems();
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
	createPanel(role);
}

async function getItems() {
	let path = "https://localhost:7063/Cart";
	cart = await getCart(path);

	console.log(cart)

	cart.forEach(carts =>{
		createCart(carts);
	})

}


function createCart(carts) {
    let cartWrapper = document.createElement("div");
    cartWrapper.classList.add("cart-wrapper"); // Add a class for styling if needed
    
    let productName = createHTMLElement("h2", carts.name);
    let price = createHTMLElement("p", carts.price + "kr");
    let articlenumber = createHTMLElement("p", "Artikelnr: " + carts.productId);
    
    cartWrapper.appendChild(productName);
    cartWrapper.appendChild(articlenumber);
    cartWrapper.appendChild(price);
    createAmount(carts, cartWrapper); // Pass the wrapper div to createAmount
    div.appendChild(cartWrapper);
}

function createAmount(carts, cartWrapper) {
    let newInput = document.createElement("input");
    newInput.type = "number";
	newInput.value = carts.amount;
    newInput.id = carts.cartId;

    newInput.style.display = "block";
    newInput.style.margin = "auto";

    cartWrapper.appendChild(document.createElement('br'));
    cartWrapper.appendChild(newInput);
    cartWrapper.appendChild(document.createElement('br'));

    newInput.addEventListener("change", event => {
        console.log(carts.cartId);
		console.log(newInput.value);
		carts.amount = parseInt(newInput.value);
        changeAmount(carts);
    });
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

async function getCart(path) {
	console.log(localStorage.getItem("GUID"));
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


async function changeAmount(carts) {
	let path = "https://localhost:7063/Cart";
	const response = await fetch(path, {
	  method: "PUT",
	  mode: "cors",
	  headers: {
		"Content-type": "application/json",
		authorization: localStorage.getItem("GUID"),
	  },
	  body: JSON.stringify(carts),
	});
	return response.status;
  }