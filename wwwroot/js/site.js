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
    var formData = new FormData();

    var sourceInput = document.getElementById("source-input");
    var files = sourceInput.files;
    formData.append('file', files[0]);

    var palletteInput = document.getElementById("pallette-input");
    files = palletteInput.files;
    formData.append('file', files[0]);

    disableButtons();

    $.ajax({
        url: '/Home/Save',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false
    }).done(function(response) {
        enableButtons();
        console.log(response);

        window.location.href = "/Home/Results?id=" + response
    }).fail(function (jqXHR, response) {
        enableButtons();
        console.log('failed');
    });
}

function disableButtons() {
    beginButton.disabled = true;
    beginButton.innerHTML = "Loading...";

    palletteInputElement.disabled = true;
    document.getElementById("pallette-span").classList.add("disabled");
    sourceInputElement.disabled = true;
    document.getElementById("source-span").classList.add("disabled");
}

function enableButtons() {
    beginButton.disabled = false;
    beginButton.innerHTML = "Begin!";

    palletteInputElement.disabled = false;
    document.getElementById("pallette-span").classList.remove("disabled");
    sourceInputElement.disabled = false;
    document.getElementById("source-span").classList.remove("disabled");
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