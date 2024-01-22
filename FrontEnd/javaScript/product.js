import { verify, logInOrLogOut, cart } from "./verify.js";

let main;
let json = [{
    "productId":1,
    "category":"katt",
    "productName":"katt",
    "productImg":"hej.png",
    "price": 50,
    "stock":23,  
    "description":"miawdmiawmdiwamfimi mimbiwmbdiawm",
    "feeding": "imfawmfiamwbim iwamibmwimbi"
},{
    "productId":1,
    "category":"katt",
    "productName":"katt",
    "price": 50,
    "productImg":"hej.png",
    "stock":23,  
    "description":"miawdmiawmdiwamfimi mimbiwmbdiawm",
    "feeding": "imfawmfiamwbim iwamibmwimbi"
},{
    "productId":1,
    "category":"katt",
    "productName":"katt",
    "productImg":"hej.png",
    "price": 50,
    "stock":23,  
    "description":"miawdmiawmdiwamfimi mimbiwmbdiawm",
    "feeding": "imfawmfiamwbim iwamibmwimbi"
},{
    "productId":1,
    "category":"katt",
    "productName":"katt",
    "productImg":"hej.png",
    "price": 50,
    "stock":23,  
    "description":"miawdmiawmdiwamfimi mimbiwmbdiawm",
    "feeding": "imfawmfiamwbim iwamibmwimbi"
},{
    "productId":1,
    "category":"katt",
    "productName":"katt",
    "productImg":"hej.png",
    "price": 50,
    "stock":23,  
    "description":"miawdmiawmdiwamfimi mimbiwmbdiawm",
    "feeding": "imfawmfiamwbim iwamibmwimbi"
},{
    "productId":1,
    "category":"katt",
    "productName":"katt",
    "productImg":"hej.png",
    "price": 50,
    "stock":23,  
    "description":"miawdmiawmdiwamfimi mimbiwmbdiawm",
    "feeding": "imfawmfiamwbim iwamibmwimbi"
},{
    "productId":1,
    "category":"katt",
    "productName":"katt",
    "productImg":"hej.png",
    "price": 50,
    "stock":23,  
    "description":"miawdmiawmdiwamfimi mimbiwmbdiawm",
    "feeding": "imfawmfiamwbim iwamibmwimbi"
},{
    "productId":1,
    "category":"katt",
    "productName":"katt",
    "price": 50,
    "productImg":"hej.png",
    "stock":23,  
    "description":"miawdmiawmdiwamfimi mimbiwmbdiawm",
    "feeding": "imfawmfiamwbim iwamibmwimbi"
},{
    "productId":1,
    "category":"katt",
    "productName":"katt",
    "productImg":"hej.png",
    "price": 50,
    "stock":23,  
    "description":"miawdmiawmdiwamfimi mimbiwmbdiawm",
    "feeding": "imfawmfiamwbim iwamibmwimbi"
},{
    "productId":1,
    "category":"katt",
    "productName":"katt",
    "productImg":"hej.png",
    "price": 50,
    "stock":23,  
    "description":"miawdmiawmdiwamfimi mimbiwmbdiawm",
    "feeding": "imfawmfiamwbim iwamibmwimbi"
},
{
    "productId":1,
    "category":"katt",
    "productName":"katt",
    "productImg":"hej.png",
    "price": 50,
    "stock":23,  
    "description":"miawdmiawmdiwamfimi mimbiwmbdiawm",
    "feeding": "imfawmfiamwbim iwamibmwimbi"
}];
let div;

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

function createProducts() {
  //let path = "https://localhost:7063/Product/AllProducts";
  // json = await getProduct(path);
  console.log(json);

  for (let product of json) {
    let article = createHTMLElement("article");
    let figure = createHTMLElement("figure");
    let category = createHTMLElement("h3", `Category: ${product.category}`);
    let productName = createHTMLElement("h2", product.productName);
    let img = createHTMLElement("img", null, { src: product.productImg, alt: product.productName });
    let price = createHTMLElement("p", `Priset är ${product.price}kr`);
    let desc = createHTMLElement("p", product.description);
    let stock = createHTMLElement("p", `in stock: ${product.stock}`);
    let feeding = createHTMLElement("p", product.feeding);
    let button = createHTMLElement("button", "KÖP", { id: product.productId });

    div.appendChild(article);
    article.appendChild(figure);
    figure.appendChild(img);
    figure.appendChild(productName);
    figure.appendChild(category);
    figure.appendChild(price);
    figure.appendChild(desc);
    figure.appendChild(feeding);
    figure.appendChild(stock);
    figure.appendChild(button);
  }
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

  let jsonData = await response.json();
  return jsonData;
}
