import {
	verify,
	logInOrLogOut,
	createPanel
} from "./verify.js";

let form;
let Json = [];

function init() {
    console.log("hej");
	getVerify();
    form = document.querySelector("form");
    
    form.addEventListener("submit", event=>{
        letFormData();
        event.preventDefault();
    })
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
	createPanel(role);
}


async function letFormData() {
    let username = form.elements.username.value;
    let role = form.elements.role.value;
    console.log(role);

    Json = {
        Role:  role, 
        username: username
    }
    console.log(Json);
    let status = await postFetch(Json);
}

async function postFetch(json){
    let path = "https://localhost:7063/User/ChangeRole";
    const response = await fetch(path, {
        method: 'PUT',
        mode: "cors",
        headers: {
            "Content-type": "application/json",
			"authorization": localStorage.getItem("GUID")
        },
        body: JSON.stringify(json)
    })
    return response.status;
}