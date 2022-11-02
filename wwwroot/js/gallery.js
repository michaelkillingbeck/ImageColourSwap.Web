var galleryCardOffHover = function() {
    var galleryInput = this.querySelector('.gallery-input');
    var galleryOutput = this.querySelector('.gallery-output');

    galleryInput.style.display = 'block';
    galleryOutput.style.display = 'none';
}

var galleryCardOnHover = function() {
    var galleryInput = this.querySelector('.gallery-input');
    var galleryOutput = this.querySelector('.gallery-output');

    galleryInput.style.display = 'none';
    galleryOutput.style.display = 'block';
}

var navigateToResults = function(id) {
    window.location.href = "/Results/Index?id=" + id;
}

var galleryCards = document.getElementsByClassName("gallery-card");

for (var i = 0; i < galleryCards.length; i++) {
    galleryCards[i].addEventListener('mouseenter', galleryCardOnHover, false);
    galleryCards[i].addEventListener('mouseleave', galleryCardOffHover, false);
}