import {
	verify,
	logInOrLogOut,
	createPanel
} from "./verify.js";
let main;
let postJson;
let blogArray = [];
let commentsArray = [];
let blog;
let comments;
let form;
let singleId;
let inputText;
let article;
let getForm;
let div;
let placement;
let submitButton;
function init() {
	const queryString = window.location.search;
	const urlParams = new URLSearchParams(queryString);
	singleId = Number(urlParams.get("Id"));
	getVerify();
	main = document.getElementsByTagName("main")[0];
	div = document.getElementsByTagName("div")[1];
	blogPost();
}
window.onload = init;

async function getVerify() {
	const role = await verify();
	logInOrLogOut(role);
	createPanel(role)
}

async function blogPost() {
    let path = "https://localhost:7063/Blog/" + singleId;
    console.log(path);
    blog = await getJson(path);
    console.log(blog);

    placement = createHTMLElement("div");
    main.appendChild(placement);

    createBlog(blog);

    const comments = blog.comments; 
    comments.forEach(comment => {
        console.log(comment);
        createComment(comment);
    });

    sendComment();
}

function createBlog(blog){
	article = document.createElement("article");
	let blogText = createHTMLElement("p", blog.text);
	let title = createHTMLElement("h3", blog.title);
	let timestamp = createHTMLElement("p", blog.time);
	div.appendChild(article);
	createfigure(blog);
	article.appendChild(title);
	article.appendChild(blogText);
	article.appendChild(timestamp);
}

function createfigure(blog) {
	let figure = createHTMLElement("figure");
	let username = createHTMLElement("h2", blog.username);
	let img = createHTMLElement("img", null, {
		src: blog.img,
		alt: blog.name
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

function createComment(comment) {
	createCommentText(comment);
	main.appendChild(placement);
}


function sendComment(){
	form = createHTMLElement("form");
	inputText = document.createElement("input");
	submitButton = document.createElement("button");
	submitButton.type = "submit";
	submitButton.innerHTML = "ladda upp kommentar";
	inputText.type="text";
	inputText.id="comment";
	inputText.name="comment"
	inputText.required = true;
	placement.appendChild(form);
	form.appendChild(inputText);
	form.appendChild(submitButton);
	submitButton.addEventListener("click", event=>{
		letFormData();
		event.preventDefault();
		location.reload();
	})
}
async function letFormData(){
	getForm = document.querySelector("form");
	console.log(getForm);
	let text = getForm.elements.comment.value;
	console.log(text);
	postJson = {
		"text":text,
		"blogId":singleId,
	}
	console.log(postJson);
	let status = await postComment(postJson);
}
function createCommentText(comment){
	let commentUser =createHTMLElement("h3", comment.username);
	let commentText = createHTMLElement("p", comment.text);
	placement.appendChild(commentUser);
	placement.appendChild(commentText);
	console.log(comment);
}

async function getJson(path){
	let response = await fetch(path, {
		headers: {
			Authorization: localStorage.getItem("GUID"),
		},
	});
	let jsonData = await response.json();
	console.log(jsonData);
	return jsonData;
}

async function postComment(postJson){
	let path = "https://localhost:7063/Comment";
	console.log(postJson);
	console.log(path);
	let response = await fetch(path,{
		method: "POST",
		mode: "cors",
		headers:{
			"Content-type": "application/json",
			"authorization": localStorage.getItem("GUID")
		},
        body: JSON.stringify(postJson)
	})
	console.log(postJson);
	return response.status;
}