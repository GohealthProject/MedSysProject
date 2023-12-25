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
    window.addEventListener('load', () => {
        aos_init();
    });

    fetch(`https://localhost:7078/api/Blogs/latest6`)
        .then(response => response.json())
        .then(data => {
            const take4 = data.$values.slice(0, 4);
            const footerContainer = document.querySelector('.footerRecent');
            const recent4 = take4.map(blog => {
                const { blogId, title, author, articleClass, createdAt, views,blogImage} = blog;
                const date = new Date(createdAt);
                const year = date.getFullYear();
                const month = String(date.getMonth() + 1).padStart(2, '0');
                const day = String(date.getDate()).padStart(2, '0');
                const formattedDate = `${year}-${month}-${day}`;
                let imageUrl = '';
                if (blog.blogImage != null)
                {
                    var binary = atob(blog.blogImage);
                    var byteArray = new Uint8Array(binary.length);
                    for (var i = 0; i < binary.length; i++)
                    {
                        byteArray[i]= binary.charCodeAt(i);
                    }
                    var blob = new Blob([byteArray], { type: 'image/jpeg' });
                    imageUrl = URL.createObjectURL(blob);
                }
                return `
                    <li>
                                <a href="@Url.Content(~/Blogs/SinglePost)?SingleBlogId=${blogId}" class="d-flex align-items-center">
                                    <img src="${imageUrl}" alt="" class="img-fluid me-3">
                                    <div>
                                        <div class="post-meta d-block"><span class="date">${articleClass}</span> <span class="mx-1">&bullet;</span> <span>${formattedDate}</span></div>
                                        <span>${title}</span>
                                    </div>
                                </a>
                            </li>
                `
            });
        footerContainer.innerHTML = recent4.join('');
        });
});