/**
* Template Name: ZenBlog
* Updated: Sep 18 2023 with Bootstrap v5.3.2
* Template URL: https://bootstrapmade.com/zenblog-bootstrap-blog-template/
* Author: BootstrapMade.com
* License: https:///bootstrapmade.com/license/
*/
document.addEventListener('DOMContentLoaded', () => {
  "use strict";

  /**
   * Sticky header on scroll
   */
    const selectHeader = document.querySelector('#header');
  /*�o�q�{���X���N��O�A�p�G selectHeader �ܼƦs�b�B���ȡ]�D undefined�Bnull �� false�^�A�h���汵�U�Ӫ��{���X�C�o�O�@�ر`�������m�ʽs�X�覡�A�H�T�O�b�� selectHeader ������ާ@���e�A�����O���w�q���ΪŭȡA�H�קK�X�{���~�C

�b�o�ӯS�w�Ҥl���A�p�G selectHeader �ܼƦs�b�A�{���X�|���[�@�Ӻu�ʺ�ť�� (addEventListener) �� document �W�A��ϥΪ̺u�ʮɡA�|�ˬd window.scrollY ���ȬO�_�j�� 100�A�p�G�O�A�h�� selectHeader �����s�W�@�ӦW�� sticked �� CSS class�A�_�h�R���o�� class�C

�o�˪�����P�_�T�O�F�b����u�ʨƥ󪺥N�X���e�AselectHeader ���s�b�ʩM�X�k�ʡA�H�קK�X�{���~�Υ��w�q���欰�C*/
  if (selectHeader) {
    document.addEventListener('scroll', () => {
      window.scrollY > 100 ? selectHeader.classList.add('sticked') : selectHeader.classList.remove('sticked');
    });
  }

  /**
   * Mobile nav toggle
   */

  const mobileNavToogleButton = document.querySelector('.mobile-nav-toggle');

  if (mobileNavToogleButton) {
    mobileNavToogleButton.addEventListener('click', function(event) {
      event.preventDefault();
      mobileNavToogle();
    });
  }

  function mobileNavToogle() {
    document.querySelector('body').classList.toggle('mobile-nav-active');
    mobileNavToogleButton.classList.toggle('bi-list');
    mobileNavToogleButton.classList.toggle('bi-x');
  }

  /**
   * Hide mobile nav on same-page/hash links
   */
  document.querySelectorAll('#navbar a').forEach(navbarlink => {

    if (!navbarlink.hash) return;

    let section = document.querySelector(navbarlink.hash);
    if (!section) return;

    navbarlink.addEventListener('click', () => {
      if (document.querySelector('.mobile-nav-active')) {
        mobileNavToogle();
      }
    });
  });

  /**
   * Toggle mobile nav dropdowns
   */
  const navDropdowns = document.querySelectorAll('.navbar .dropdown > a');

  navDropdowns.forEach(el => {
    el.addEventListener('click', function(event) {
      if (document.querySelector('.mobile-nav-active')) {
        event.preventDefault();
        this.classList.toggle('active');
        this.nextElementSibling.classList.toggle('dropdown-active');

        let dropDownIndicator = this.querySelector('.dropdown-indicator');
        dropDownIndicator.classList.toggle('bi-chevron-up');
        dropDownIndicator.classList.toggle('bi-chevron-down');
      }
    })
  });

  /**
   * Scroll top button
   */
  const scrollTop = document.querySelector('.scroll-top');
  if (scrollTop) {
    const togglescrollTop = function() {
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
   * Hero Slider
   */
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

  /**
   * Open and close the search form.
   */
  const searchOpen = document.querySelector('.js-search-open');
  const searchClose = document.querySelector('.js-search-close');
  const searchWrap = document.querySelector(".js-search-form-wrap");

  searchOpen.addEventListener("click", (e) => {
    e.preventDefault();
    searchWrap.classList.add("active");
  });

  searchClose.addEventListener("click", (e) => {
    e.preventDefault();
    searchWrap.classList.remove("active");
  });

  /**
   * Initiate glightbox
   */
  const glightbox = GLightbox({
    selector: '.glightbox'
  });

  /**
   * Animation on scroll function and init
   */
  function aos_init() {
    AOS.init({
      duration: 1000,
      easing: 'ease-in-out',
      once: true,
      mirror: false
    });
  }
  window.addEventListener('load', () => {
    aos_init();
  });

});