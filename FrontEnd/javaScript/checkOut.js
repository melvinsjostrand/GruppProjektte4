let main;
let div
function init(){
    main = document.getElementsByTagName("main")[0];
    getItems();
}
window.onload = init;

function getItems(){
    div = document.getElementsByTagName("div")[1];
    console.log(div)
    for(i = 0; i < 1; i++){
        let productName = document.createElement("h2");
        let price = document.createElement("p");
        let select = document.createElement("select");
        for(i = 1; i < 11; i++){
            let option = document.createElement("option");
            option.value = i;
            option.innerHTML = i;
            select.appendChild(option);
        }
        div.appendChild(productName);
        div.appendChild(price);
        div.appendChild(select);
    }
}
function details(){

}