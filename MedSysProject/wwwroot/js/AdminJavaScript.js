document.addEventListener("DOMContentLoaded", function (event) {

    const showNavbar = (toggleId, navId, bodyId, headerId) => {
        const toggle = document.getElementById(toggleId),
            nav = document.getElementById(navId),
            bodypd = document.getElementById(bodyId),
            headerpd = document.getElementById(headerId)

        // Validate that all variables exist
        if (toggle && nav && bodypd && headerpd) {
            toggle.addEventListener('click', () => {
                // show navbar
                nav.classList.toggle('show')
                // change icon
                toggle.classList.toggle('bx-x')
                // add padding to body
                bodypd.classList.toggle('body-pd')
                // add padding to header
                headerpd.classList.toggle('body-pd')
            })
        }
    }

    showNavbar('header-toggle', 'nav-bar', 'body-pd', 'header')

    /*===== LINK ACTIVE =====*/
    const linkColor = document.querySelectorAll('.nav_link')

    function colorLink() {
        if (linkColor) {
            linkColor.forEach(l => l.classList.remove('active'))
            this.classList.add('active')
        }
    }
    linkColor.forEach(l => l.addEventListener('click', colorLink))

    // Your code to run since DOM is loaded and ready
});

var myLink = document.querySelectorAll('a[href="#"]');
myLink.forEach(function (link) {
    link.addEventListener('click', function (e) {
        e.preventDefault();
    });
});

const logout = () => {
    Swal.fire({
        title: "確定要登出嗎?",
        icon: "question",
        confirmButtonText: "確定",
        cancelButtonText: "取消",
        showCancelButton: true,
        showCloseButton: true
    });
}

Swal.fire({
    position: "top-end",
    icon: "success",
    title: "您已成功登入！",
    showConfirmButton: false,
    timer: 1500
});