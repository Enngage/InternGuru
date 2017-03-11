require(['modules/coreModule', 'jquery', 'semantic'], function (CoreModule, $) {
    // DOM ready
    var coreModule = new CoreModule();

    $(function () {
        // init tab
        var anchorId = coreModule.getQueryStringValue("s");
        if (!anchorId) {
            anchorId = $("#_SectionAnchor").data("anchor");
        }

        if (anchorId) {
            $('html, body').animate({
                scrollTop: $("#" + anchorId).offset().top - 54 // 54 = offset from fixed header
            }, 350);
        }

        // fixed top scrolling menu after 
        $('#_CompanyMenuWrapper')
            .visibility({
                type: 'fixed'
                //offset: 54 // fixed by CSS 
            });

        $("#_CompanyMenuWrapper a").on('click',
            function (e) {
                e.preventDefault();
                var anchorId = $(this).data("anchor");
                if (!anchorId) {
                    return;
                }
                $('html, body').animate({
                    scrollTop: $("#" + anchorId).offset().top - 54 // 54 = offset from fixed header
                }, 350);
            });
    });
});