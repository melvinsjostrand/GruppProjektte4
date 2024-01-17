let role;
function init(){
    verify();
    logInOrLogOut();
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
    console.log(role);
    if(role == 2){
        console.log("Du är inloggad som admin");
    }else if(role == 1){
        console.log("Du är inloggad som vanlig användare");
    }else{
        console.log("Du är inte inloggad");
    }
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
        location.href = "logout.html"
    }else{
        link.innerText = "Logga in";
        location.href = "login.html"
    }
}