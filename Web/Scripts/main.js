require(['jquery', 'views/shared/menuSearch', 'semantic'], function ($, menuSearch) {

    $(function () {
        // mobile toggle
        $('._MobileMenu ._MobileMenuToggle').on("click", function (e) {
            e.preventDefault();
            $('._MobileMenu .ui.vertical.menu').toggle();
        });

        // lazy load images
        $(".image").visibility({
            type: 'image',
            transition: 'fade in',
            duration: 400
        });
    });
});