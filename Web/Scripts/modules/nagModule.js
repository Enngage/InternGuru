define(['modules/coreModule', 'jquery', 'promise'], function (Coremodule, $, ExtendedPromise) {

    function NagModule() {
    }

    var Promise = ExtendedPromise.Promise;

    NagModule.prototype.initNag = function (nagElementId, cookieName) {
        // NOTE:
        // Nag has to be hidden using CSS first

        var nagElem = $("#" + nagElementId);

        // check if cookie is set
        var cookieValue = ($.cookie(cookieName) === 'true');
        if (cookieValue !== true) {
            // show nag
            nagElem.show();

            // handle clicks
            $("#" + nagElementId + " #_CloseNag").on('click',
                function (e) {
                    e.preventDefault();

                    // set cookie
                    $.cookie(cookieName, true, { expires: 365, path: '/' });

                    nagElem.hide();
                });
        }
    }

    return NagModule;
});