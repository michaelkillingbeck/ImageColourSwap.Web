var beginButton = document.getElementById("begin-button");
var palletteInputElement = document.getElementById("pallette-input");
var sourceInputElement = document.getElementById("source-input");

palletteInputElement.onchange = function(event) {
    var canvas = document.getElementById("pallette-image");
    loadImage(this, canvas);
}

sourceInputElement.onchange = function(event) {
    var canvas = document.getElementById("source-image");
    loadImage(this, canvas);
}

beginButton.onclick = function(event) {
    var sourceCanvas = document.getElementById("source-image");
    var sourceDataString = sourceCanvas.toDataURL("image/png");
    var palletteCanvas = document.getElementById("pallette-image");
    var palletteDataString = palletteCanvas.toDataURL("image/png");

    $.ajax({
        type: "POST",
        url: "Home/Save",
        data: { 
           sourceImage: sourceDataString,
           palletteImage: palletteDataString
        }
      }).done(function(o) {
        console.log('saved'); 
      });
}

function disableButton() {
    beginButton.disabled = true;
    beginButton.innerHTML = "Loading...";
}

function enableButton() {
    beginButton.disabled = false;
    beginButton.innerHTML = "Begin!";
}

function loadImage(input, canvas) {
    var file, fr, img;
    
    file = input.files[0];
    fr = new FileReader();
    fr.onload = createImage;
    fr.readAsDataURL(file);

    function createImage() {
        img = new Image();
        img.onload = imageLoaded;
        img.src = fr.result;
    }

    function imageLoaded() {
        canvas.width = img.width;
        canvas.height = img.height;
        var ctx = canvas.getContext("2d");
        ctx.drawImage(img,0,0);
    }
}