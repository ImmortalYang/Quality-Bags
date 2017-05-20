/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// identity function for calling harmony imports with the correct context
/******/ 	__webpack_require__.i = function(value) { return value; };
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, {
/******/ 				configurable: false,
/******/ 				enumerable: true,
/******/ 				get: getter
/******/ 			});
/******/ 		}
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 0);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


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
        $.get("/Products/List?categoryId=" + categoryId, function (data) {
            $(".product-list[data-category-id='" + categoryId + "']").html(data);
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
    $(".add-to-cart").off("click").click(function () {
        //Ajax call AddToCart action in ShoppingCart controller
        $.get("/ShoppingCart/AddToCart/" + $(this).data("product-id"), function (data) {
            //update returned shopping cart view component in the container
            refreshShoppingCart(data);
        });
    });
}

function addClickEventListenerToRemoveFromCartBtns() {
    $(".remove-from-cart").off("click").click(function () {
        //Ajax call RemoveFromCart action in ShoppingCart controller
        $.get("/ShoppingCart/RemoveFromCart/" + $(this).data("product-id"), function (data) {
            //update returned shopping cart view component in the container
            refreshShoppingCart(data);
        });
    });
}

function addClickEventListenerToEmptyCartBtn() {
    $(".empty-cart").off("click").click(function () {
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
    var contentString = "<div><h4>Quality Bags</h4><a href = '' target = '_blank'>Visit our website</a></div>";

    var infowindow = new google.maps.InfoWindow({
        content: contentString
    });

    marker.addListener('click', function () {
        infowindow.open(map, marker);
    });
}

/***/ })
/******/ ]);