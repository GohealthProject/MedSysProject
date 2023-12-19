document.addEventListener('DOMContentLoaded', () => {
    var logbutton = document.querySelector('#logout');

    if (logbutton) {
        logbutton.addEventListener('click', () => {
            Swal.fire({
                title: "您已登出!",
                text: "將為您跳轉至首頁",
                icon: "success",
                showConfirmButton: false,
                timer: 1500
            });
        })
    }
})





