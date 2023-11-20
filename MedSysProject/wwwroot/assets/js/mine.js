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
    /*Mobile Nav Toggle*/
    const mobileNavToggleBtn = document.querySelector('.mobile-nav-toggle');
    if (mobileNavToggleBtn) {/*如果按鈕存在*/
        mobileNavToggleBtn.addEventListener('click', function (event) {
            event.preventDefault();
            mobileNavToggle();
        });
    }

    function mobileNavToggle() {
        document.querySelector('body').classList.toggle('mobile-nav-active');
        mobileNavToggleBtn.classList.toggle('bi-list');
        mobileNavToggleBtn.classList.toggle('bi-x');
    }
});