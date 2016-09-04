define(['jquery'], function ($) {

    function General() {
    }

    General.prototype.numberWithCommas = function numberWithCommas(number) {
        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    // I know its twice here...
    General.prototype.formatNumber = function numberWithCommas(number) {
        return this.numberWithCommas(number);
    }

    General.prototype.roundNumber = function numberWithCommas(number) {
        return Math.round(number * 100) / 100;
    }

    General.prototype.replaceAll = function (text, search, replacement) {
        return text.replace(new RegExp(search, 'g'), replacement);
    };

    General.prototype.changeQueryStringValue = function (uri, key, value) {
        var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
        var separator = uri.indexOf('?') !== -1 ? "&" : "?";
        if (uri.match(re)) {
            return uri.replace(re, '$1' + key + "=" + value + '$2');
        }
        else {
            return uri + separator + key + "=" + value;
        }
    }

    return General;
});