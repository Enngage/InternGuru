require(['jquery', 'datepicker', 'ckeditor', 'semantic' ], function ($, Datepicker) {
    // DOM ready
    $(function () {
        // initialization
        $('._DropdownSearch, ._CurrencyDropdown, ._DurationDropdown, ._StateDropdown, ._CategorySearchDropdown').dropdown();
        InitializeDatePicker();
        SetInitialState();
        InitializeCkEditor();

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
            CKEDITOR.replace('_Description', {
                customConfig: '/scripts/addons/ckeditor/simpleEditorConfig.js'
            });
        }

        function ShowAmountField() {
            var amountFieldID = "_InternshipAmountField";
            $("#" + amountFieldID).removeClass("w-body-hide");
        }

        function HideAmountField() {
            var amountFieldID = "_InternshipAmountField";
            $("#" + amountFieldID).addClass("w-body-hide");
        }

        function InitializeDatePicker() {
            $('._Datepicker').datepicker();
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