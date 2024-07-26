function Hamburger() {
    var hamburger = document.querySelector('.hamburger');
    var menu = document.querySelector('.menu-container');
    var logo = document.querySelector('.logo');

    var isMenuVisible = false;

    hamburger.addEventListener('click', function () {
        if (!isMenuVisible) {
            menu.style.display = 'block';
            menu.style.transform = 'translateY(0)';
            isMenuVisible = true;
            logo.style.display = 'none';
        } else {
            menu.style.transform = 'translateY(-100vh)';
            isMenuVisible = false;
            logo.style.display = 'block';

        }
    });

}

Hamburger();

//function CardHandler() {
//    let currentImageIndex = 0;
//    const images = [
//        '/img/Card1.jpg',
//        '/img/Card3.png',
//        '/img/Card2.jpg',
//    ];

//    function changeBackgroundImage() {
//        const imageContainer = document.querySelector('.card');
//        currentImageIndex = (currentImageIndex + 1) % images.length;
//        imageContainer.style.backgroundImage = `url(${images[currentImageIndex]})`;
//    }

//    const cards = document.querySelectorAll('.card');
//    cards.forEach(card => {
//        card.addEventListener('click', changeBackgroundImage);
//    });

//}

CardHandler();
