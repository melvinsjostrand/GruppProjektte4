let role;
let main;
let controller;

function init() {
	verify();
}
window.onload = init;

async function verify() {
	let userverify = "https://localhost:7063/User/Verify";
	let response = await fetch(userverify, {
		headers: {
			"Authorization": localStorage.getItem("GUID")
		}
	});
	role = await response.text();
	console.log(role)
	if (role == 2) {
		console.log("Du är inloggad som admin");
	} else if (role == 1) {
		console.log("Du är inloggad som vanlig användare");
	} else {
		console.log("Du är inte inloggad");
	}
	return role;
}

function logInOrLogOut() {
	let nav = document.getElementsByTagName("nav")[0];
	let ul = document.getElementsByTagName("ul")[0];
	let li = document.getElementsByTagName("li")[3];
	let link = document.createElement("a");
	nav.appendChild(ul);
	ul.appendChild(li);
	li.appendChild(link);
	if (role == 2) {
		link.innerText = "logga ut";
		link.href = "logout.html"
	} else if (role == 1) {
		link.innerText = "logga ut"
		link.href = "logout.html";
	} else {
		link.innerText = "Logga in";
		link.href = "login.html"
	}
}

function createPanel() {
	let nav = document.getElementsByTagName("nav")[0];
	let ul = document.getElementsByTagName("ul")[0];
	let li = document.createElement("li");
	let panelButton = document.createElement("a");
	ul.appendChild(li);
	li.appendChild(panelButton);
	panelButton.href = "controller.html";
	if (role == 1) {
		panelButton.innerHTML = "AnvändarPanel"
	} else if (role == 2) {
		panelButton.innerHTML = "AdminPanel"
	} else {
		panelButton.style.display = "none";
	}
}

function seeBlogButtons() {
	let main = document.getElementsByTagName("main")[0];
	let blogButton = document.createElement("a");
	let location;
	main.appendChild(blogButton);
	if (role == 1) {
		blogButton.innerHTML = "skapa blogg";
		blogButton.href = "createBlog.html";
	} else {
		blogButton.style.display = "none";
	}
}

function panelControl() {
	main = document.getElementsByTagName("main")[0];
	if (role == 1) {
		userPanel();
	} else if (role == 2) {
		adminPanel();
	} else {
		let span = document.createElement("span");
		main.appendChild(span);
		span.innerHTML("ERROR");
	}
}

function userPanel() {
	let a = document.getElementsByTagName("a");
	for (let i = 0; i < 2; i++) {
		controller = document.createElement("a");
		main.appendChild(controller);
	}
	a[4].innerHTML = "Ändra lösenord";
	a[4].href = "changePassword.html";
	a[5].innerHTML = "Ta bort blogginlägg";
	a[5].href = "removeBlogText1.html";
}

function adminPanel() {
	let a = document.getElementsByTagName("a");
	for (let i = 0; i < 6; i++) {
		controller = document.createElement("a");
		main.appendChild(controller);
	}
	a[4].innerHTML = "Ändra produkt";
	a[4].href = "updateProduct.html";
	a[5].innerHTML = "Lägg till produkt";
	a[5].href = "uploadProduct.html";
	a[6].innerHTML = "Ändra roller";
	a[6].href = "changeRole.html";
	a[7].innerHTML = "Ta bort blogginlägg";
	a[7].href = "removeBlogText.html";
	a[8].innerHTML = "Ta bort Produkt";
	a[8].href = "removeProduct.html";
	a[9].innerHTML = "Ta bort användare";
	a[9].href = "deleteUser.html";
}

function cart() {
	main = document.getElementsByTagName("main")[0];
	let a = document.createElement("a");
	main.appendChild(a);
	a.innerHTML = "Kassa";
	a.href = "checkOut.html";
}

export {
	verify,
	logInOrLogOut,
	createPanel,
	seeBlogButtons,
	panelControl,
	cart
};