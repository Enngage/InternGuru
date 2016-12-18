require(['jquery', 'unitegallery', 'unitegallery_theme'], function ($) {
    // DOM ready
    $(function () {
        // get gallery ID
        var galleryID = $('#_GalleryID').data("id");

        // gallery
        $('#' + galleryID).unitegallery();
    });
}); 