require(['jquery', 'datepicker', 'ckeditor', 'semantic', ], function ($, Datepicker) {
    // DOM ready
    $(function () {
        // initialization
        $('._ThesisTypeDropdown, ._ThesisCategoryDropdown, ._CurrencyDropdown').dropdown();
        InitializeCkEditor();
        SetInitialState();

        $('#_IsActiveCheckbox').checkbox();

        // checkbox events
        $('#_IsPaidCheckbox').checkbox({
            onChecked: function () {
                // show field
                ShowAmountField();
            },
            onUnchecked: function () {
                // hide field
                HideAmountField();
            }
        });

        function InitializeCkEditor() {
            CKEDITOR.replace('_ThesisDescription', {
                customConfig: '/scripts/addons/ckeditor/simpleEditorConfig.js'
            });
        }

        function ShowAmountField() {
            var amountFieldID = "_ThesisAmountFieldWrapper";
            $("#" + amountFieldID).removeClass("w-body-hide");
        }

        function HideAmountField() {
            var amountFieldID = "_ThesisAmountFieldWrapper";
            $("#" + amountFieldID).addClass("w-body-hide");
        }

        function SetInitialState() {
            var paidChecked = $('#_IsPaidCheckbox').checkbox("is checked");
            if (paidChecked) {
                ShowAmountField();
            }
            else {
                HideAmountField();
            }
        }

    });
});