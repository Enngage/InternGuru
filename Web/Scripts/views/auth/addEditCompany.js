require(['jquery', 'datepicker', 'ckeditor', 'semantic', ], function ($, Datepicker) {
    // DOM ready
    $(function () {
        // initialization
        $('._YearFoundedDropdown, ._CompanySizeDropdown, ._StateDropdown, ._CompanyCategoryDropdown').dropdown();
        InitializeCkEditor();

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
            CKEDITOR.replace('_LongDescription', {
                customConfig: '/scripts/addons/ckeditor/simpleEditorConfig.js'
            });
        }

    });
});