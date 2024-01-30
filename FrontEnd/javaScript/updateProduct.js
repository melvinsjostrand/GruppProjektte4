import { verify, logInOrLogOut, createPanel } from "./verify.js";

let form;
let select;
let content;
let feeding;
let json;
let Json;
let labelsAfterSelect;
let srcdata;
let Id;
let div;
let article;
function init() {
  form = document.querySelector("form");
  content = form.elements.content;
  let productId = form.elements.productId;
  div = document.getElementsByTagName("div")[1];
  feeding = form.elements.feeding;
  labelsAfterSelect = form.querySelectorAll("label");
  console.log(labelsAfterSelect);

  select = form.elements.category;
  console.log(select.value);
  selectedValue();
  getVerify();
  form.addEventListener("submit", (event) => {
    letFromData();
    event.preventDefault();
  });

  productId.addEventListener("change", event=>{
    Id = form.elements.productId.value;
    while (div.firstChild) {
			div.removeChild(div.firstChild);
		  }      
    createProducts();
  })
  select.addEventListener("change", (event) => {
    selectedValue();
  });
}
window.onload = init;

async function getVerify() {
  const role = await verify();
  logInOrLogOut(role);
  createPanel(role);
}

async function createProducts() {
	let path = "https://localhost:7063/Product/ProductId?Id=" + Id;
  console.log(path);
	json = await fetchData(path);
	console.log(json);
if(json == 0){
  console.log("Produkten finns inte");
  createError();
}else{
	json.forEach(product => {
		createArticle(product);
	});
}

}

async function letFromData() {
  Id = form.elements.productId.value;
  let name = form.elements.productname.value;
  let desc = form.elements.desc.value;
  let category = select.value;
  let price = form.elements.price.value;
  let stock = form.elements.stock.value;
  content = form.elements.content.value;
  feeding = form.elements.feeding.value;
  Json = {
    Id: Id,
    price: price,
    category: category,
    name: name,
    Img: srcdata,
    description: desc,
    stock: stock,
    content: content,
    feeding: feeding,
  };
  console.log(Json);
  let status = await updateProduct(Json);
}

const fileInput = document.getElementById("fileinput");

fileInput.addEventListener("change", (e) => {
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
    labelsAfterSelect[5].style.display = "block";
    labelsAfterSelect[6].style.display = "block";
  } else {
    content.style.display = "none";
    feeding.style.display = "none";
    labelsAfterSelect[5].style.display = "none";
    labelsAfterSelect[6].style.display = "none";
  }
}


function createError(){
  let error = document.createElement("span");
  error.innerHTML = "PRODUKTEN FINNS INTE";
  div.appendChild(error);
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
  
  article.appendChild(category);
  article.appendChild(price);
  article.appendChild(desc);
  article.appendChild(rating);
  article.appendChild(articlenumber);
  article.appendChild(feeding);
  article.appendChild(inStock);
  div.appendChild(article);
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

async function updateProduct(json) {
  let path = "https://localhost:7063/Product/UpdateProduct";
  const response = await fetch(path, {
    method: "PUT",
    mode: "cors",
    headers: {
      "Content-type": "application/json",
      authorization: localStorage.getItem("GUID"),
    },
    body: JSON.stringify(json),
  });
  // If the request is successful, no errors will be thrown and the promise resolves
  return response.status;
}


async function fetchData(path){
  let response = await fetch(path, {
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