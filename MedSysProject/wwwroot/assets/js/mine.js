document.addEventListener('DOMContentLoaded', () => {
    "use strict";

    /*sticky header效果*/
    const header = document.querySelector('#header');
    console.log(header);
    if (header) {
        document.addEventListener('scroll', () => {
            window.scrollY > 100 ? header.classList.add('sticked') : header.classList.remove('sticked');
        });
    }

});