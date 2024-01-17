let main;
let json = []
function init(){
    createlogin();
    main = document.getElementsByTagName("main")[0];
    createBlog();
    console.log(json);
}
window.onload = init;

function createBlog(){
    for(i = 0; i< json.length; i++){
    let article = document.createElement("article");
    let figure = document.createElement("figure");
    let title = document.createElement("h2");
    let username = document.createElement("h3");
    let timestamp = document.createElement("p");
    main.appendChild(article)
    article.appendChild(figure);
    figure.appendChild(title);
    title.innerHTML = json[i].name;
    figure.appendChild(username);
    figure.appendChild(timestamp);


    username.innerHTML = json[i].age;
    timestamp.innerHTML = json[i].city;
    }
    article.style.cursor = "pointer";
    article.addEventListener("click", event=>{
        location.href ="bloginfo.html?="+json[i].blogId;
    })
}

async function getJson(){
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