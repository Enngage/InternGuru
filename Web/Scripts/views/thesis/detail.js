require(['jquery', 'semantic'], function ($) {
    // DOM ready
    $(function () {
        // fixed top scrolling menu  
        $('#_ThesisMenuWrapper')
            .visibility({
                type: 'fixed',
                //offset: 54 // fixed by CSS 
            });
    });
});