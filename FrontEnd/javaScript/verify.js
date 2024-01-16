function init(){
    verify();
}
window.onload = init;

async function verify(){
    let userverify = "https://localhost:7063/User/Verify";
    let response = await fetch(userverify, {
    headers:{
        "Authorization": localStorage.getItem("GUID")
    }
    });

    let role = await response.text();
    console.log(role);
    if(role == 2){
        console.log("Du är inloggad som admin");
    }else if(role == 1){
        console.log("Du är inloggad som vanlig användare");
    }else{
        console.log("Du är inte inloggad");
    }
}