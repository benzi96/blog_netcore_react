// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(window).on('scroll', function (event) {
    var scrollValue = $(window).scrollTop();
    if (scrollValue > 250) {
        $('.transparentnavbar').addClass('affix');
        $('.transparentnavbar').removeClass('affix-top');
    } else {
        $('.transparentnavbar').removeClass('affix');
        $('.transparentnavbar').addClass('affix-top');
    }
});

$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});

$(function () {
    $('#myEditor').ckeditor();
});

