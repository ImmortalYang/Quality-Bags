$(document).ready(function () {
    $(".img-input").change(function () {
        readURL(this);
    });

    addClickEventListenerToAddToCartBtns();
    addClickEventListenerToRemoveFromCartBtns();
    addClickEventListenerToEmptyCartBtn();
});

//Show the change of uploaded image in the browser
function readURL(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('.product-image').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

function addClickEventListenerToAddToCartBtns() {
    $(".add-to-cart")
        .off("click")
        .click(function () {
        //Ajax call AddToCart action in ShoppingCart controller
        $.get("/ShoppingCart/AddToCart/" + $(this).data("product-id"), function (data) {
            //update returned shopping cart view component in the container
            refreshShoppingCart(data);
        });
    });
}

function addClickEventListenerToRemoveFromCartBtns() {
    $(".remove-from-cart")
        .off("click")
        .click(function () {
        //Ajax call RemoveFromCart action in ShoppingCart controller
        $.get("/ShoppingCart/RemoveFromCart/" + $(this).data("product-id"), function (data) {
            //update returned shopping cart view component in the container
            refreshShoppingCart(data);
        });
    });
}

function addClickEventListenerToEmptyCartBtn() {
    $(".empty-cart")
        .off("click")
        .click(function () {
        //Confirm dialog
        if (false === confirm("Are you sure you want to delete all the items in the shopping cart?")) {
            return;
        }
        //Ajax call EmptyCart action in ShoppingCart controller
        $.get("/ShoppingCart/EmptyCart", function (data) {
            //update returned shopping cart view component in the container
            refreshShoppingCart(data);
        });
    });
}

function refreshShoppingCart(data) {
    $("#shopping-cart").html(data);
    addClickEventListenerToAddToCartBtns();
    addClickEventListenerToRemoveFromCartBtns();
    addClickEventListenerToEmptyCartBtn();
}