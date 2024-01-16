let main;
let json = []
function init(){
    main = document.getElementsByTagName("main")[0];
    createBlog();
}
window.onload = init;

function createBlog(){
    for(i = 0; i< json; i++){
    let section = document.createElement("section");
    let figure = document.createElement("figure");
    let title = document.createElement("h2");
    let username = document.createElement("h3");
    let timestamp = document.createElement("p");
    section.appendChild(figure);
    figure.appendChild(title);
    figure.appendChild(username);
    figure.appendChild(timestamp);
    }
}

async function JSON(){
    let path = "https://localhost:7063/Blog/AllBlog";
    json = await getblog(path);
    console.log(json);
}

async function getblog(path){
    console.log(localStorage.getItem("GUID"));
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