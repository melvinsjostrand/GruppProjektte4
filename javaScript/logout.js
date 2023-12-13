function init(){
    let guid = localStorage.getItem("GUID");
    console.log(guid);

    localStorage.removeItem("GUID");

    console.log(localStorage.getItem("GUID"));

location.href = "index.html";
}
window.onload = init;