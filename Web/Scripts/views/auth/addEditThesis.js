require(['jquery', 'datepicker', 'ckeditor', 'semantic', ], function ($, Datepicker) {
    // DOM ready
    $(function () {
        // initialization
        $('._ThesisTypeDropdown, ._ThesisCategoryDropdown, ._CurrencyDropdown, ._QuestionnaireDropdown').dropdown();
        initializeCkEditor();
        setInitialState();

        $('#_IsActiveCheckbox, #_HideAmountCheckbox').checkbox();

        // checkbox events
        $('#_IsPaidCheckbox').checkbox({
            onChecked: function () {
                // show field
                showAmountField();
            },
            onUnchecked: function () {
                // hide field
                hideAmountField();
            }
        });

        // checkbox events
        $('#_HideAmountCheckbox').checkbox({
            onChecked: function () {
                // hide field
                hideAmountField();
           
            },
            onUnchecked: function () {
                // show field
                showAmountField();
            }
        });


        function initializeCkEditor() {
            CKEDITOR.replace('_ThesisDescription', {
                customConfig: '/scripts/addons/ckeditor/simpleEditorConfig.js'
            });
        }

        function showAmountField() {
            var amountFieldID = "_ThesisAmountFieldWrapper";
            $("#" + amountFieldID).removeClass("w-body-hide");
        }

        function hideAmountField() {
            var amountFieldID = "_ThesisAmountFieldWrapper";
            $("#" + amountFieldID).addClass("w-body-hide");
        }

        function setInitialState() {
            var paidChecked = $('#_IsPaidCheckbox').checkbox("is checked");
            var hideAmountChecked = $('#_HideAmountCheckbox').checkbox("is checked");

            if (paidChecked) {
                if (hideAmountChecked) {
                    hideAmountField();
                } else {
                    showAmountField();
                }
            }
            else {
                hideAmountField();
            }
        }

    });
});