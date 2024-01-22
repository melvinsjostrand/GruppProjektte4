import {verify, logInOrLogOut} from "./verify.js";
let div;
function init(){
    getVerify();
    div = document.getElementsByTagName("div")[1];
    getItems();
}
window.onload = init;

async function getVerify(){
    const role = await verify();
    logInOrLogOut(role);
}

async function getItems(){
    console.log(div)
    for(Element of json){
        let productName = document.createElement("h2");
        let price = document.createElement("p");
        let select = document.createElement("select");
        div.appendChild(productName);
        div.appendChild(price);
        div.appendChild(select);
        for(j = 0; j < 10; j++){
            let option = document.createElement("option");
            option.value = 1 + j;
            option.innerHTML = 1 + j;
            select.appendChild(option);
            console.log(option);
        }

    }
}
