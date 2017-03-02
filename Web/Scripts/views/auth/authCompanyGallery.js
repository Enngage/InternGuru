require(['jquery', 'modules/companyGalleryModule', 'semantic'], function ($, CompanyGalleryModule) {
    // DOM ready
    $(function () {
        var companyGalleryModule = new CompanyGalleryModule();

        var configElem = $("#_CompanyGalleryConfig");
        var companyGuid = configElem.data("guid");

        // initialize semantic ui cards
        $('._CompanyGalleryCards .image').dimmer({
            on: 'hover'
        });

        $('._CompanyGalleryCards ._DeleteGallery').click(function () {
            var fileName = $(this).data("filename");
            var cardColumn = hideCard($(this).closest('.column'));

            companyGalleryModule.deleteImage(companyGuid, fileName).then(cardColumn, function () {

                hideCard(cardColumn);

            }, function (error) {
                alert("Nastala chyba");
                console.error("Failed!", error);
            });
        });

        function hideCard(target) {
            $(target).remove();
        }
    });
});