let main;
let button;
json = [{
    "productId" : 1,
    "price" : 100,
    "productImg" : "hej.png",
    "productName" : "string(32)",
    "category":"katt",
    "description": "This is a test product.",
    },{
        "productId" : 2,
        "price" : 100,
        "productImg" : "hej.png",
        "productName" : "string(32)",
        "category":"katt",
        "description": "This is a test product.",
    }
]
function init(){
    main = document.getElementsByTagName("main")[0];
    createProduct();
    console.log(json);
    console.log(button);

}
window.onload = init();

function createProduct(){
    for(i = 0; i< json.length; i++){
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

        main.appendChild(article);
        article.appendChild(figure);
        figure.appendChild(img);
        figure.appendChild(productName);
        figure.appendChild(category);
        figure.appendChild(price);
        figure.appendChild(desc);
        figure.appendChild(stock);
        figure.appendChild(feeding);
        figure.appendChild(button);

        productName.innerHTML = json[i].productName;
        price.innerHTML = json[i].price + "kr";
        img.src=json[i].productImg;
        category.innerHTML ="Category: "+json[i].category;
        desc.innerHTML= json[i].description;
        stock.innerHTML="in stock: "+json[i].stock;
        feeding.innerHTML="Feeding instructions: "+json[i].feedingInstructions
        //Add event listener to the add to cart button
        button.id = json[i].productId
        button.innerHTML = "KÖP";
        button.addEventListener("click", event=>{
            console.log(button.id);
            event.preventDefault();
        })
        }
}


async function getJson(){
    let path = "https://localhost:7063/Blog/AllBlog";
    json = await getProduct(path);
    console.log(json);
}

async function getblog(path){
    console.log(localStorage.getItem("GUID"));
    let response = await fetch(path, {
        headers:{
            "Authorization": localStorage.getItem("GUID")
        }
    });
    let json = await response.json();
    return json;
}

function addToCart(){

    Json = {
        
        };
    let status = addToCart(Json);
    console.log(status);
}