import {verify, logInOrLogOut, seeBlogButtons} from "./verify.js";

let main;
let div;
let json = [
]
function init(){
    main = document.getElementsByTagName("main")[0];
    div = document.getElementsByTagName("div")[1];
    createBlog();
    getVerify();
}
window.onload = init;

async function getVerify(){
    const role = await verify();
    logInOrLogOut(role);
    seeBlogButtons(role);
}


async function createBlog(){
    let path = "https://localhost:7063/Blog/AllBlog";
    json = await getblog(path);
    console.log(json);
    let article;
    for(Element of json){
        article = document.createElement("article");
        let figure = document.createElement("figure");
        let title = document.createElement("h2");
        let img = document.createElement("img");
        let username = document.createElement("figcaption");
        let timestamp = document.createElement("p");
        article.appendChild(title);
        figure.appendChild(img);
        figure.appendChild(username);
        article.appendChild(figure);
        article.appendChild(timestamp);
        div.appendChild(article);
        title.innerHTML = Element.title;
        img.src = Element.blogImg;
        username.innerHTML=Element.username;
        timestamp.innerHTML = Element.time;
        let Id = Element.blogId;
        article.style.cursor = "pointer";
        article.href = "bloginfo.html?="+Id;
    }
}


async function getblog(path){
    let response = await fetch(path, {
        headers:{
            "Authorization": localStorage.getItem("GUID")
        }
    });
    if(response.status!==200){
        return "error";
    }
    let json = await response.json();
    return json;
}