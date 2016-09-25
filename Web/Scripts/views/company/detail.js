require(['jquery', 'modules/companyModule', 'semantic'], function ($, CompanyModule) {
    // DOM ready
    $(function () {
        var companyModule = new CompanyModule();

        // menu variables
        var rightMenuID = '_CompanyRightMenu';

        // initialize tabs menu
        $('#' + rightMenuID + ' .item').tab();

        // fixed top scrolling menu after 
        $('#_CompanyMenuWrapper')
            .visibility({
                type: 'fixed',
                //offset: 54 // fixed by CSS 
            });

        // scrolling on menu item click 
        $('#' + rightMenuID + ' .item').click(function () {
            $('html, body').animate({
                scrollTop: $("#_CompanyScrollAnchor").offset().top
            }, 300);
        });
    });
});