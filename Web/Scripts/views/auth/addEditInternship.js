require(['jquery', 'datepicker', 'ckeditor', 'semantic' ], function ($, Datepicker) {
    // DOM ready
    $(function () {
        // initialization
        $('._DropdownSearch, ._CurrencyDropdown, ._DurationDropdown, ._StateDropdown, ._CategorySearchDropdown, ._LanguagesDropdown, ._HomeOfficeOptionDropdown, ._StudentStatusOptionDropdown').dropdown();
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

        // checkbox events
        $('#_HasFlexibleHours').checkbox({
            onChecked: function () {
                // hide field
                HideWorkingHoursField();
            },
            onUnchecked: function () {
                // show field
                ShowWorkingHoursField();
            }
        });

        function InitializeCkEditor() {
            CKEDITOR.replace('_Description', {
                customConfig: '/scripts/addons/ckeditor/simpleEditorConfig.js'
            });

            CKEDITOR.replace('_Requirements', {
                customConfig: '/scripts/addons/ckeditor/simpleEditorConfig.js'
            });
        }

        function ShowWorkingHoursField() {
            var fieldID = "_WorkingHoursField";
            $("#" + fieldID).removeClass("w-body-hide");
        }

        function HideWorkingHoursField() {
            var fieldID = "_WorkingHoursField";
            $("#" + fieldID).addClass("w-body-hide");
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

            var hasFlefibleHours = $('#_HasFlexibleHours').checkbox("is checked");
            if (hasFlefibleHours) {
                HideWorkingHoursField();
            }
            else {
                ShowWorkingHoursField();
            }
        }

    });
});