define(['modules/coreModule', 'jquery', 'promise', 'semantic'], function (Coremodule, $, ExtendedPromise) {

    function ModalModule() {
    }

    var Promise = ExtendedPromise.Promise;

    ModalModule.prototype.initDeleteModal = function initDeleteModal(modalSelector, clickElementSelector, approveFunction, hideElemAfterDeleteSelector) {
        var idDataName = "id";
        var deleteObjectID = 0;
        var hideElement = null;

        // init modal
        $(modalSelector)
            .modal({
                closable: false,
                onDeny: function () {
                    return true;
                },
                onApprove: function () {
                    // execute approve function
                    approveFunction(deleteObjectID).then(function () {
                        // hide item
                        $(hideElement).remove();

                    }, function (error) {
                        alert("Nastala chyba");
                        console.error("Failed!", error);
                    });
                }
            });

        // hook up click action for opening modal
        $(clickElementSelector).click(function (e) {
            e.preventDefault();

            deleteObjectID = $(this).data(idDataName);
            hideElement = $(this).closest(hideElemAfterDeleteSelector);
            ModalModule.prototype.showModal(modalSelector);
        });
    }

    ModalModule.prototype.showModal = function showModal(modalSelector) {
        $(modalSelector).modal('show');
    }

    return ModalModule;
});