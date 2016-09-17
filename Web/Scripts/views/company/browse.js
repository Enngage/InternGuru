require(['jquery', 'modules/companyModule', 'semantic'], function ($, CompanyModule) {
    $(function () {
        var companyModule = new CompanyModule();
        var defaultPageNumber = 2; // First set of results is pre-loaded
        var currentPageNumber = defaultPageNumber; // variable to hold current page number (updated by each load)
        var search = ""; // search placeholder

        // hide loading
        HideLoading();

        // hide companies result
        HideAllCompaniesLoaded();

        // hide no companies found
        HideNoCompaniesFound();

        $("#_SearchCompanies").bind('input', function () {
            // hide no companies found
            HideNoCompaniesFound();

            // add loading to input box
            $(this).parent().addClass("loading");

            // get searched value
            var searchFor = $("#_SearchCompanies").val();

            // assign search value
            search = searchFor;

            // reset current page to 1 when searching
            currentPageNumber = 1;

            companyModule.getMoreCompanies(currentPageNumber, search).then(function (companies) {
                var html = "";

                // handle no results
                if (companies.length === 0) {
                    // clear companies
                    $("#_CompanySegment").html(html);

                    // hide loading
                    $("#_SearchCompanies").parent().removeClass("loading");

                    // show no companies found
                    ShowNoCompaniesFound();

                    // hide all companies loaded
                    HideAllCompaniesLoaded();
                    return;
                }

                // add companies
                for (i = 0; i < companies.length; i++) {
                    var company = companies[i];
                    html += GetCompanyHtml(company);
                }

                $("#_CompanySegment").html(html);

                HideLoading();

                // update current page number
                currentPageNumber += 1;

                // hide loading
                $("#_SearchCompanies").parent().removeClass("loading");

            }, function (error) {
                HideLoading();
                alert("Nastala chyba");
                console.error("Failed!", error);
            });
        });

        $('#_CompanySegment')
            .visibility({
                once: false,
                // update size when new content loads
                observeChanges: true,
                // load content on bottom edge visible
                onBottomVisible: function () {
                    // hide no companies found if search is not used
                    if (!search) {
                        HideNoCompaniesFound();
                    }

                    // show loading
                    ShowLoading();

                    // load companies
                    companyModule.getMoreCompanies(currentPageNumber, search).then(function (companies) {
                        var html = "";

                        // handle no results
                        if (companies.length === 0) {
                            HideLoading();
                            // show all companies loaded if page != 1
                            if (currentPageNumber !== 1) {
                                ShowAllCompaniesLoaded();
                            }
                            return;
                        }

                        // add companies
                        for (i = 0; i < companies.length; i++) {
                            var company = companies[i];
                            html += GetCompanyHtml(company);
                        }

                        $("#_CompanySegment").append(html);

                        HideLoading();

                        // update current page number
                        currentPageNumber += 1;

                    }, function (error) {
                        HideLoading();
                        alert("Nastala chyba");
                        console.error("Failed!", error);
                    });
                }
            });
    });

    function GetCompanyHtml(company) {
        var html = "";

        html += '<div class="sixteen wide mobile sixteen wide tablet sixteen wide computer w-color-background-white column w-padding-none">';
        html += '<div class="ui segment basic padded w-margin-none w-body-text-left">';
        html += '<div class="ui items">';
        html += '<div class="item">';
        html += '<img class="ui image tiny w-company-logo" alt="Logo ' + company.CompanyName + '"  src="' + company.LogoImageUrl + '" />';
        html += '  <div class="middle aligned content">';
        html += '         <div class="header">';
        html += '              <a href="' + company.Url + '">';
        html += '                   ' + company.CompanyName;
        html += '               </a>';
        html += '            </div>';
        html += '            <div class="meta">';
        html += '               ' + company.CountryIcon + ' ' + company.City + ' | <a href="' + company.UrlToInternships + '">' + company.InternshipCount + ' ' + company.PluralInternshipsCountWord + '</a>';
        html += '            </div>';
        html += '        </div>';
        html += '      </div>';
        html += '   </div>';
        html += ' </div>';
        html += ' <a href="' + company.Url + '">';
        html += '     <img class="ui image w-company-browse-banner" alt="Banner ' + company.CompanyName + '" src="' + company.BannerImageUrl + '" />';
        html += ' </a>';
        html += '</div>';

        return html;
    }


    function ShowLoading() {
        $("#_CompanyLoading").show();
    }

    function HideLoading() {
        $("#_CompanyLoading").hide();
    }

    function HideAllCompaniesLoaded() {
        $("#_AllCompaniesLoaded").hide();
    }

    function ShowAllCompaniesLoaded() {
        $("#_AllCompaniesLoaded").show();
    }

    function HideNoCompaniesFound() {
        $("#_NoCompaniesFound").hide();
    }

    function ShowNoCompaniesFound() {
        $("#_NoCompaniesFound").show();
    }
});