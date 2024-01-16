let form;
function init(){
    form = document.querySelector("form");
    form.addEventListener("submit", event=>{
        getData();
        event.preventDefault();
    })
}
window.onload = init;
async function getData(){
    let username = form.elements.username.value;
    let mail = form.elements.mail.value;
    let password = form.elements.password.value;

    json ={
        "mail":mail,
        "password":password
    }
    let status = await postFetch(json)
}

async function postFetch(json){
    let path = "https://localhost:7063/User";
    console.log( localStorage.getItem("GUID"));
    let mail = form.elements.mail.value;
    let password = form.elements.password.value;
    console.log(mail);
    console.log(password);
    const response = await fetch(path ,{
        method:"POST",
        mode:"cors",
        headers:{
            "content-type":"application/json",
          "Authorization": "Basic: " + btoa(mail+":"+password)
        },
        body:JSON.stringify(json)
    })
   return response.status;
}