function Hamburger(){
    var hamburger = document.querySelector('.hamburger');
    var menu = document.querySelector('.menu-container');
    var logo = document.querySelector('.logo');

var isMenuVisible = false;

hamburger.addEventListener('click', function(){
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