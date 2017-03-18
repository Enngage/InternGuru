require(['jquery', 'datepicker', 'ckeditor', 'semantic'], function ($, Datepicker) {
    // DOM ready
    $(function () {
        // initialization
        $('._DropdownSearch, ._CurrencyDropdown, ._DurationDropdown, ._StateDropdown, ._CategorySearchDropdown, ._LanguagesDropdown, ._MinEducationTypeDropdown, ._StudentStatusOptionDropdown, ._QuestionnaireDropdown').dropdown();
        initializeDatePicker();
        setInitialState();
        initializeCkEditor();

        $('#_IsActiveCheckbox').checkbox();

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
        $('#_HasFlexibleHours').checkbox({
            onChecked: function () {
                // hide field
                hideWorkingHoursField();
            },
            onUnchecked: function () {
                // show field
                showWorkingHoursField();
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
            CKEDITOR.replace('_Description', {
                customConfig: '/scripts/addons/ckeditor/simpleEditorConfig.js'
            });

            CKEDITOR.replace('_Requirements', {
                customConfig: '/scripts/addons/ckeditor/simpleEditorConfig.js'
            });
        }

        function showWorkingHoursField() {
            var fieldID = "_WorkingHoursField";
            $("#" + fieldID).removeClass("w-body-hide");
        }

        function hideWorkingHoursField() {
            var fieldID = "_WorkingHoursField";
            $("#" + fieldID).addClass("w-body-hide");
        }

        function showAmountField() {
            var amountFieldID = "_InternshipAmountField";
            $("#" + amountFieldID).removeClass("w-body-hide");
        }

        function hideAmountField() {
            var amountFieldID = "_InternshipAmountField";
            $("#" + amountFieldID).addClass("w-body-hide");
        }

        function initializeDatePicker() {
            $('._Datepicker').datepicker();
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

            var hasFlefibleHours = $('#_HasFlexibleHours').checkbox("is checked");
            if (hasFlefibleHours) {
                hideWorkingHoursField();
            }
            else {
                showWorkingHoursField();
            }
        }

    });
});