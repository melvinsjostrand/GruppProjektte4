let role;
let main;
function init(){
    verify();
}
window.onload = init;

async function verify(){
    let userverify = "https://localhost:7063/User/Verify";
    let response = await fetch(userverify, {
    headers:{
        "Authorization": localStorage.getItem("GUID")
    }
    });
    role = await response.text();
    console.log(role)
    if(role == 2){
        console.log("Du är inloggad som admin");
    }else if(role == 1){
        console.log("Du är inloggad som vanlig användare");
    }else{
        console.log("Du är inte inloggad");
    }
    return role;
}

function logInOrLogOut(){
    let nav = document.getElementsByTagName("nav")[0];
    let ul = document.getElementsByTagName("ul")[0];
    let li = document.getElementsByTagName("li")[3];
    let link = document.createElement("a");
    nav.appendChild(ul);
    ul.appendChild(li);
    li.appendChild(link);
    if(role == 2){
        link.innerText = "logga ut";
        link.href = "logout.html"
    }else if(role == 1){
        link.innerText = "logga ut"
        link.href = "logout.html";
    }
    else{
        link.innerText = "Logga in";
        link.href = "login.html"
    }
}

function createPanel(){
    let main = document.getElementsByTagName("main")[0];
    let panelButton = document.createElement("a");
    main.appendChild(panelButton);
    panelButton.href = "controller.html";
    if(role == 1){
        panelButton.innerHTML = "AnvändarPanel"
    }else if(role == 2){
        panelButton.innerHTML = "AdminPanel"
    }else{
        panelButton.style.display = "none";
    }
}

function seeBlogButtons(){
    let main = document.getElementsByTagName("main")[0];
    let blogButton = document.createElement("a");
    let location;
    main.appendChild(blogButton);
    if (role == 1){
        blogButton.innerHTML = "skapa blogg";
        blogButton.href = "createBlog.html";
    }else if(role == 2){
        blogButton.innerHTML = "Ta bort blogg";
        blogButton.href =  "removeBlogText.html";
    }else{
        blogButton.style.display="none";
    }
    console.log(location);
}

function panelControl(){
    main = document.getElementsByTagName("main")[0];
    if(role == 1){
        userPanel();
    }else if(role == 2){
        adminPanel();
    }else{
        let span = document.createElement("span");
        main.appendChild(span);
        span.innerHTML("ERROR");
    }
}

function userPanel(){
    let controller;
    let a = document.getElementsByTagName("a");
    for(let i = 0; i < 2; i++){
        controller = document.createElement("a");
        main.appendChild(controller);
    }
    a[4].innerHTML = "Ändra lösenord";
    a[4].href = "changePassword.html";
    a[5].innerHTML = "Ta bort blogginlägg";
    a[5].href = "removeBlogText.html";
}

function adminPanel(){
    let controller;
    let a = document.getElementsByTagName("a");
    for(let i = 0; i < 4; i++){
        controller = document.createElement("a");
        main.appendChild(controller);
    }
    a[4].innerHTML = "Ta bort eller ändra produkt";
    a[4].href = "changeProduct.html";
    a[5].innerHTML = "Lägg till produkt";
    a[5].href = "addProduct.html";
    a[6].innerHTML = "Ändra roller";
    a[6].href = "changeRole.html";
    a[7].innerHTML = "Ta bort blogginlägg";
    a[7].href = "removeBlogText.html";
}

function cart(){
    main = document.getElementsByTagName("main")[0];
    let a = document.createElement("a");
    main.appendChild(a);
    a.innerHTML = "Kassa";
    a.href = "checkOut.html";
}

export {verify , logInOrLogOut, createPanel, seeBlogButtons, panelControl, cart};

