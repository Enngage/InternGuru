require(['jquery', 'modules/nagModule', 'semantic', 'jquery_cookie'], function ($, NagModule) {

    $(function () {
        var nagModule = new NagModule();

        // mobile toggle
        $('._MobileMenu ._MobileMenuToggle').on("click", function (e) {
            e.preventDefault();
            $('._MobileMenu .ui.vertical.menu').toggle();
        });

        // lazy load images
        $(".image").visibility({
            type: 'image',
            transition: 'fade in',
            duration: 400,
            offset: 48 + 20 // offset for height of the fixed menu + 20 px (just to make them appear sooner) = makes images appear on time
        });

        // cookie consent
        nagModule.initNag("_CookieConsent", "accept-cookies");
    });
});