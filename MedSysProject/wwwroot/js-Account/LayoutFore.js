document.addEventListener('DOMContentLoaded', () => {
    var logbutton = document.querySelector('#logout');

    if (logbutton) {
        logbutton.addEventListener('click', () => {
            alert("登出成功");
        })
    }
})





