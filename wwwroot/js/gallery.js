var GalleryCardOffHover = function() {
    var galleryInput = this.querySelector('.gallery-input');
    var galleryOutput = this.querySelector('.gallery-output');

    galleryInput.style.display = 'block';
    galleryOutput.style.display = 'none';
}

var GalleryCardOnHover = function() {
    var galleryInput = this.querySelector('.gallery-input');
    var galleryOutput = this.querySelector('.gallery-output');

    galleryInput.style.display = 'none';
    galleryOutput.style.display = 'block';
}

var galleryCards = document.getElementsByClassName("gallery-card");

for (var i = 0; i < galleryCards.length; i++) {
    galleryCards[i].addEventListener('mouseenter', GalleryCardOnHover, false);
    galleryCards[i].addEventListener('mouseleave', GalleryCardOffHover, false);
}