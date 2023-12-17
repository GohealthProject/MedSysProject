document.addEventListener('DOMContentLoaded', () => {
    "use strict";

    /*sticky header效果*/
    const header = document.querySelector('#header');
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
    let what = document.querySelectorAll('#navbar a');

    //console.log(what);//F12可以用小箭頭看具體是那些元素

    //下拉式選單，點擊後箭頭朝上
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

    //移至頂端按鈕
    const scrollTop = document.querySelector('.scroll-top');
    if (scrollTop) {
        const togglescrollTop = function () {
            window.scrollY > 100 ? scrollTop.classList.add('active') : scrollTop.classList.remove('active');
        }
        window.addEventListener('load', togglescrollTop);
        document.addEventListener('scroll', togglescrollTop);
        scrollTop.addEventListener('click', window.scrollTo({
            top: 0,
            behavior: 'smooth'
        }));
    }

    /**
 * Initiate glightbox
 */
    const glightbox = GLightbox({
        selector: '.glightbox'
    });

    const searchOpen = document.querySelector('.js-search-open');
    console.log(searchOpen);//抓到了
    const searchClose = document.querySelector('.js-search-close');
    const searchWrap = document.querySelector(".js-search-form-wrap");
    const searchForm = document.querySelector('.search-form');
    searchOpen.addEventListener("click", (e) => {
        e.preventDefault();
        console.log("Hi");
        searchWrap.classList.add("active");
        searchForm.addEventListener('keydown', function (e) {
            if (e.key === 'Enter')
            {
                e.preventDefault();
                searchForm.submit();
            }
        });
    });

    searchClose.addEventListener("click", (e) => {
        e.preventDefault();
        searchWrap.classList.remove("active");
    });

    function aos_init() {
        AOS.init({
            duration: 1000,
            easing: 'ease-in-out',
            once: true,
            mirror: false
        });
    }
});