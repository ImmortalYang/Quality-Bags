// Write your Javascript code.
function readURL(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('.product-image').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$(".img-input").change(function () {
    readURL(this);
});