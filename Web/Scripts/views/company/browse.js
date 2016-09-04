require(['jquery', 'modules/companyModule', 'semantic'], function ($, CompanyModule) {
    // DOM ready
    $(function () {
        var companyModule = new CompanyModule();

        // hide loading
        HideLoading();

        $('#_CompanySegment')
            .visibility({
                once: false,
                // update size when new content loads
                observeChanges: true,
                // load content on bottom edge visible
                onBottomVisible: function () {
                    ShowLoading();

                    // load companies
                    companyModule.getMoreCompanies().then(function (companies) {
                        var html = "";

                        // add companies
                        for (i = 0; i < companies.length; i++) {
                            var company = companies[i];

                            html += '<div class="sixteen wide mobile eight wide tablet five wide computer column">';
                            html += '<a href="#">';
                            html += '<img class="ui image fluid" alt="Company name image" src="http://semantic-ui.com/images/wireframe/image.png" />';
                            html += '</a>';
                            html += '<div class="w-body-text-left">';
                            html += company.CompanyName;
                            html += '</a>';
                            html += '</div>';
                            html += '</div>';
                        }

                        $("#_CompanySegment").append(html);

                        HideLoading();

                    }, function (error) {
                        HideLoading();
                        alert("Nastala chyba");
                        console.error("Failed!", error);
                    });
                }
            });
    });


    function ShowLoading() {
        $("#_CompanyLoading").show();
    }

    function HideLoading() {
        $("#_CompanyLoading").hide();
    }
});