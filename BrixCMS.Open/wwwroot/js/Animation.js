
//-------------carouselContainer---------------

document.addEventListener('DOMContentLoaded', () => {
    const carouselContainer = document.querySelector('#@unique');
    if (!carouselContainer) return;

    const carouselContent = carouselContainer.querySelector('#carouselContent');
    if (!carouselContent) return;

    let currentIndex = 0;
    const items = carouselContent.querySelectorAll('.carousel-item');
    const totalItems = items.length;

    function moveCarousel() {
        currentIndex = (currentIndex + 1) % totalItems;
        carouselContent.style.transform = `translateX(-${currentIndex * 100}%)`;
    }

    setInterval(moveCarousel, 3000);
});