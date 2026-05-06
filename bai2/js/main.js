/* global jQuery */
(function ($) {
    'use strict';

    $(function () {
        var $hero = $('#heroSlider');
        if ($hero.length) {
            // Ensure carousel behaves consistently across browsers.
            $hero.carousel({
                interval: 3500,
                pause: 'hover',
                ride: 'carousel',
                wrap: true,
                keyboard: true
            });
        }

        // Toggle active menu item when user clicks on nav links.
        $('.navbar-nav .nav-link').on('click', function () {
            $('.navbar-nav .nav-item').removeClass('active');
            $(this).closest('.nav-item').addClass('active');
        });
    });
})(jQuery);

