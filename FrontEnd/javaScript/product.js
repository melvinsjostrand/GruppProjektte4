import {verify, logInOrLogOut, cart} from "./verify.js";
let main;
let Id;
let button;
let div;
let json = []
function init(){
    createProduct();
    getVerify();
    main = document.getElementsByTagName("main")[0];
    div = document.getElementsByTagName("div")[1];
}
window.onload = init;

async function getVerify(){
    const role = await verify();
    logInOrLogOut(role);
    cart(role);
}

async function createProduct(){
    let path = "https://localhost:7063/Product/AllProducts";
    json = await getProduct(path);
    console.log(json);

    for(let i = 0; i< json.length; i++){
        let article = document.createElement("article");
        let figure = document.createElement("figure");
        let category = document.createElement("h3");
        let productName = document.createElement("h2");
        let img = document.createElement("img");
        let price = document.createElement("p");
        let desc = document.createElement("p");
        let stock = document.createElement("p");
        let content = document.createElement("p");
        let feeding = document.createElement("p");
        button = document.createElement("button");



        figure.appendChild(img);
        figure.appendChild(productName);
        article.appendChild(category);
        article.appendChild(price);
        article.appendChild(desc);
        article.appendChild(stock);
        article.appendChild(feeding);
        article.appendChild(button);
        article.appendChild(figure);
        div.appendChild(article);
        productName.innerHTML = json[i].productName;
        price.innerHTML = "Priset är " + json[i].price + "kr";
        img.src=json[i].productImg;
        category.innerHTML ="Category: "+json[i].category;
        desc.innerHTML= json[i].description;
        stock.innerHTML="in stock: "+json[i].stock;
        feeding.innerHTML= json[i].feeding
        Id = json[i].productId;
        button.id = Id;
        button.innerHTML = "KÖP";
    }
}

async function getProduct(path){
    console.log(localStorage.getItem("GUID"));
    let response = await fetch(path, {
        headers:{
            "Authorization": localStorage.getItem("GUID")
        }
    });
    let json = await response.json();
    return json;
}