require(['jquery', 'datepicker', 'ckeditor', 'semantic', ], function ($, Datepicker) {
    // DOM ready
    $(function () {
        // initialization
        $('._YearFoundedDropdown, ._CompanySizeDropdown, ._StateDropdown, ._CompanyCategoryDropdown').dropdown();
        InitializeCkEditor();

        function InitializeCkEditor() {
            CKEDITOR.replace('_LongDescription', {
                customConfig: '/scripts/addons/ckeditor/simpleEditorConfig.js'
            });
        }

    });
});