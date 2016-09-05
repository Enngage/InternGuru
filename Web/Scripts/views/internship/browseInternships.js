require(['jquery', 'modules/internshipModule', 'semantic'], function ($, InternshipModule) {
    // DOM ready
    $(function () {
        var internshipModule = new InternshipModule();

        $('._BrowseDropdown').dropdown();
    });
});