let role;
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
    console.log("din roll är " + role);
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
    if(role != 0){
        link.innerText = "logga ut";
        link.href = "logout.html"
    }else{
        link.innerText = "Logga in";
        link.href = "login.html"
    }
}

function createPanel(){
    let main = document.getElementsByTagName("main")[0];
    let panelButton = document.createElement("button");
    main.appendChild(panelButton);
    if(role == 1){
        panelButton.innerHTML = "AnvändarPanel"
    }else if(role == 2){
        panelButton.innerHTML = "AdminPanel"
    }else{
        panelButton.style.display = "none";
    }
}

export {verify , logInOrLogOut, createPanel};

