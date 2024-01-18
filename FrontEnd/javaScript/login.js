import {verify, logInOrLogOut} from "./verify.js";
let form;
let error;
function init(){
    form = document.querySelector("form");
    getVerify();
    form.addEventListener("submit", event=>{
        login();
        event.preventDefault();
    })
}
window.onload = init;

async function getVerify(){
    const role = await verify();
    logInOrLogOut(role);
}

async function login(){
    let mail = form.elements.mail.value;
    let password = form.elements.password.value;
    error = document.getElementById("error");
    let path = "https://localhost:7063/User/Login"

    const response = await fetch(path ,{
        method:"GET",
        mode:"cors",
        headers:{
          Authorization: "Basic: " + btoa(mail+":"+password)
        }
    })
    if(response.status === 200){
        let key = await response.text();
        //inloggad
        localStorage.setItem("GUID" , key)
        location.href = "/FrontEnd/index.html";
    } else if (response.status === 400) {
        //error lösenord/mail fel
        console.log("error");
        error.innerHTML = "-Felaktig mail/telefon/lösenord";
    } else{
        error.innerHTML = "-Det gick inte att logga in";
    }
}