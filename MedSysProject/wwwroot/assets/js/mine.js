document.addEventListener('DOMContentLoaded', () => {
    "use strict";
    
    /*sticky header效果*/
    const header = document.querySelector('#header');
    console.log(header);
    console.log("我是layout");
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
    /*不太懂 hash是什麼?*/
    document.querySelectorAll('#navbar a').forEach(navbarlink => {
        if (!navbarlink.hash) return;
        let section = document.querySelector(navbarlink.hash);
        if (!section) return;
        navbarlink.addEventListener('click', () => {
            if (document.querySelector('.mobile-nav-active')) {
                mobileNavToggle();
            }
        })
    })
    const navDropdowns = document.querySelectorAll('.navbar .dropdown >a');
    navDropdowns.forEach(el => {
        el.addEventListener('click', function (event) {
            if (document.querySelector('.mobile-nav-active')) {
                event.preventDefault();
                this.classList.toggle('active');
                this.nextElementSibling.classList.toggle('dropdown-active');
                let dropDownIndicator = this.querySelector('.dropdown-indicator');
                dropDownIndicator.classList.toggle('bi-chevron-up');
                dropDownIndicator.classList.toggle('bi-chevron-down');
            }
        })    
    })
    /*輪播牆*/
    var swiper = new Swiper(".sliderFeaturedPosts", {
        spaceBetween: 0,
        speed: 500,
        centeredSlides: true,
        loop: true,
        slideToClickedSlide: true,
        autoplay: {
            delay: 3000,
            disableOnInteraction: false,
        },
        pagination: {
            el: ".swiper-pagination",
            clickable: true,
        },
        navigation: {
            nextEl: ".custom-swiper-button-next",
            prevEl: ".custom-swiper-button-prev",
        },
    });

});