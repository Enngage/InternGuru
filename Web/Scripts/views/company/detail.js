require(['jquery', 'modules/companyModule', 'semantic'], function ($, CompanyModule) {
    // DOM ready
    $(function () {
        var companyModule = new CompanyModule();
       
        $('.menu .item').tab();

    });
});