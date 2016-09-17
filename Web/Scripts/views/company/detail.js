require(['jquery', 'modules/companyModule', 'semantic'], function ($, CompanyModule) {
    // DOM ready
    $(function () {
        var companyModule = new CompanyModule();

        $('#_CompanyRightMenu .item').tab();

        // fixed top scrolling menu after 
        $('#_CompanyMenuWrapper')
            .visibility({
                type: 'fixed',
                //offset: 54 // height of top fixed menu
            });
    });
});