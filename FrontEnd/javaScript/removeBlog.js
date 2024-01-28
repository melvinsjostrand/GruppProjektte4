import {
	verify,
	logInOrLogOut,
	createPanel
} from "./verify.js";
let url = "https://localhost:7063/Blog/DeleteBlogAdmin";
let main;
let div;
let blogs = [];
let article;

function init() {
	main = document.getElementsByTagName("main")[0];
	div = document.getElementsByTagName("div")[1];
	createblogs();
	getVerify();
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
	createPanel();
}

async function createblogs() {
	let path = "https://localhost:7063/Blog/AllBlog";
	blogs = await getblog(path);
	console.log(blogs);

	blogs.forEach(blog => {
		createArticle(blog);
	});
}

function createArticle(blog) {
	article = createHTMLElement("article");
	createfigure(blog);
	let title = createHTMLElement("h2", `Category: ${blog.title}`);
	let timestamp = createHTMLElement("p", `Priset är ${blog.time}`);
    let button = createHTMLElement("button", "Ta bort", {
		id: blog.id
	});
    button.addEventListener("click", event => {
		console.log("blog Id", blog.id);
        deleteProduct(blog.id);
	})
	article.appendChild(title);
	article.appendChild(timestamp);
    article.appendChild(button);
	div.appendChild(article);
}

function createfigure(blog) {
	let figure = createHTMLElement("figure");
	let username = createHTMLElement("figcaption", blog.username);
	let img = createHTMLElement("img", null, {
		src: blog.img,
		alt: blog.username
	});
	figure.appendChild(img);
	figure.appendChild(username);
	article.appendChild(figure);

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

async function getblog(path) {
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

async function deleteProduct(Id) {
    try {
        let confirmation = confirm("Are you sure you want to delete this blog?");
        if (confirmation) {
            let deleteResponse = await deletefetch(Id);
            if (deleteResponse >= 200 && deleteResponse < 300) {
                alert("Product deleted successfully!");
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

async function deletefetch(Id) {
    let deleteProduct = { id: Id };
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