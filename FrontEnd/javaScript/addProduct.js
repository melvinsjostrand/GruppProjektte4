import {verify, logInOrLogOut} from "./verify.js";
let form;
let select;

function init(){
    form = document.querySelector("form");
    select = document.getElementsByTagName("select")[0];
    getVerify();
    form.addEventListener("submit", event=>{
        letFromData();
        event.preventDefault();
    })
}
window.onload = init;

async function getVerify(){
    const role = await verify();
    logInOrLogOut(role);
}

async function letFromData(){
    let productname = form.elements.productname.value;
    let desc = form.elements.desc.value;
    let category = select.value;
}

async function postFetch(json){
    let path = "https://localhost:7063/product";
    const response = await fetch(path, {
        method: 'POST',
        mode:"cors",
        headers:{
            "Content-type":"application/json",
            "authorization":  localStorage.getItem("GUID")
        },
        body:JSON.stringify(json)
    })
    return response.status;
}