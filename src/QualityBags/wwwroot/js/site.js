import $ from "jquery";

google.maps.event.addDomListener(window, 'load', initializeMap);
$(document).ready(function () {
    
    $(".img-input").change(function () {
        readURL(this);
    });

    addClickEventListenerToAddToCartBtns();
    addClickEventListenerToRemoveFromCartBtns();
    addClickEventListenerToEmptyCartBtn();

    $(".view-products").click(function () {
        var categoryId = $(this).data("category-id");
        $.get("/Products/List?categoryId=" + categoryId,
            function (data) {
                $(".product-list[data-category-id='" +
                    categoryId + "']")
                    .html(data);
                addClickEventListenerToAddToCartBtns();
            });
    });
});

//Show the change of uploaded image in the browser
function readURL(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('.product-image').attr('src', e.target.result);
        };

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

//Initialize google map
function initializeMap() {
    if ($("#map")[0] === undefined) {
        return;
    }
    var coordinate = new google.maps.LatLng(-36.880851, 174.709545);
    var mapCanvas = document.getElementById('map');
    var mapOptions = {
        center: coordinate,
        zoom: 15,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var map = new google.maps.Map(mapCanvas, mapOptions);
    var marker = new google.maps.Marker({
        position: coordinate,
        map: map,
        title: "Quality Bags"
    });
    var contentString =
    "<div><h4>Quality Bags</h4><a href = '' target = '_blank'>Visit our website</a></div>";

    var infowindow = new google.maps.InfoWindow({
        content: contentString
    });

    marker.addListener('click', function () {
        infowindow.open(map, marker);
    });

}