﻿require(['jquery', 'semantic'], function ($) {
    // DOM ready
    $(function () {
        // fixed top scrolling menu after 
        $('#_CompanyMenuWrapper')
            .visibility({
                type: 'fixed'
                //offset: 54 // fixed by CSS 
            });
    });
});