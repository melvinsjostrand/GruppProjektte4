import { verify, logInOrLogOut, createPanel } from "./verify.js";
let users;
let main;
let url = "https://localhost:7063/User";
function init(){
    main = document.querySelector("main");
    getVerify();
    getUser();
}
window.onload = init;

async function getUser(){
    let path = "https://localhost:7063/User/AllUsers";
    users = await getAllUser(path);
    console.log(users);
    users.forEach(user=>{
        showUser(user);
    })
}

async function getVerify() {
    const role = await verify();
    logInOrLogOut(role);
    createPanel(role);
  }


function showUser(user){
    let section = createHTMLElement("section");
    let username = createHTMLElement("h2", user.username);
    let button = createHTMLElement("button", "Ta bort", {username: user.id});
    button.addEventListener("click", event=>{
        console.log(user.id);
        deleteUser(user.id);
    })
    main.appendChild(section);
    section.appendChild(username);
    section.appendChild(button);
}

function createHTMLElement(tag, text = null, attributes = {}) {
	let element = document.createElement(tag);

	if (text !== null) {
		element.innerHTML = text;
	}

	for (let key in attributes) {
		element.setAttribute(key, attributes[key]);
	}

	return element;
}

async function getAllUser(path){
    let response = await fetch(path, {
		headers: {
			"Authorization": localStorage.getItem("GUID")
		}
	});
	if (response.status !== 200) {
		return "error";
	}
	let json = await response.json();
	return json;
}

async function deleteUser(id) {
    try {
        let confirmation = confirm("Are you sure you want to delete this blog?");
        if (confirmation) {
            let deleteResponse = await deletefetch(id);
            if (deleteResponse >= 200 && deleteResponse < 300) {
                alert("Product deleted successfully!");
				location.reload();
                // You may want to update the UI here to reflect the deletion
            } else {
                alert("Error deleting product. Please try again.");
            }
        }
    } catch (error) {
        console.error("Error deleting product:", error);
        alert("Error deleting product. Please try again.");
    }
}

async function deletefetch(id) {
    let deleteProduct = { id: id };
    let response = await fetch(url, {
        method: 'DELETE',
        headers: {
            "Content-type": "application/json",
            "Authorization": localStorage.getItem("GUID")
        },
        body: JSON.stringify(deleteProduct)
    });
    return response.status;
}