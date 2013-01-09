/******************************************************/
/******************************************************/
// jquery extensions

$.fn.setToSizeOfWindow = function () {
    return this.each(function () {
        $(this).css({ width: $(window).width(), height: $(document).height() });
    });
};

$.fn.centerInScreen = function (width, height) {
    width = width || '85%';
    height = height || '85%';
    return this.each(function () {
        var element = $(this);
        element.css({ width: width, height: height });
        var top = ($(window).height() - element.outerHeight()) / 2;
        var left = ($(window).width() - element.outerWidth()) / 2;
        element.css({ top: top, left: left });
    });
};

$.fn.resetUnobtrusiveValidation = function () {
    return this.each(function () {
        var form = $(this);
        form.removeData('validator');
        form.removeData('unobtrusiveValidation');
        form.find('[data-valmsg-summary=true]').addClass('validation-summary-valid').removeClass('validation-summary-errors').find('ul').empty();
        $.validator.unobtrusive.parse(form);
    });
};

$.fn.appendValidationErrors = function (errors) {
    return this.each(function () {
        var container = $(this).find('[data-valmsg-summary=true]'), list = container.find('ul');
        container.addClass('validation-summary-errors').removeClass('validation-summary-valid');
        $.each(errors, function (i, val) {
            $('<li />').html(val).appendTo(list);
        });
    });
};

$.fn.toObject = function () {
    /// <summary>Only call on form elements or form input elements</summary>

    var obj = {};

    this.each(function () {
        $.map($(this).serializeArray(), function (n) {
            obj[n.name] = n.value;
        });
    });

    return obj;
};



/******************************************************/
/******************************************************/
// application namespace

app = {

    webroot: '/', // override in _Layout for dynamic web root

    modules: {}, // add modules here for consistency and convenience in the debugger

    debug: function (message) {
        if (window.console && 'debug' in window.console)
            console.debug(message);
        else if (window.console && 'log' in window.console)
            console.log(message);
        else
            $('<div>').text(message).appendTo('body');
        return this;
    },

    message: (function () {
        var _messageDiv;
        function getMessageDiv() {
            if (!_messageDiv)
                _messageDiv = $('<div>')
                    .addClass('app-message')
                    .css({ position: 'absolute', top: '0', left: '0', width: '100%', 'border-bottom': 'solid 2px #000', 'background-color': '#eaeaea', 'font-size': '1.2em', 'font-weight': 'bold', 'z-index': '100' })
                    .append($('<div>')
                        .css({ padding: '5px' })
                        .append($('<span>'))
                        .append($('<a href="#delete" title="delete">')
                            .addClass('app-button app-button-icon-solo ui-state-default ui-corner-all')
                            .css({ 'float': 'right' })
                            .hover(function () { $(this).addClass('ui-state-hover'); }, function () { $(this).removeClass('ui-state-hover'); })
                            .click(function (e) { e.preventDefault(); $(this).parents('.app-message:first').remove(); repositionMessages(); })
                            .append('<span class="ui-icon ui-icon-closethick"></span>')));
            return _messageDiv.clone(true);
        }

        function repositionMessages() {
            var lastBottom = 0;
            $('.app-message').each(function () {
                var msg = $(this);
                msg.animate({ top: lastBottom });
                lastBottom = lastBottom + msg.outerHeight();
            });
        }

        return function (message) {
            getMessageDiv().appendTo('body').find('span:first').text(message);
            repositionMessages();
            return this;
        };
    })(),

    overlay: (function () {
        var _overlayDiv;
        function getOverlayDiv() {
            if (!_overlayDiv)
                _overlayDiv = $('<div>').css({ position: 'absolute', top: '0', left: '0', 'z-index': '100', 'background-color': '#000', cursor: 'wait', filter: 'alpha(opacity=85)', '-moz-opacity': '0.85', opacity: '0.25', display: 'none' }).appendTo('body');
            return _overlayDiv;
        }

        return {
            show: function () { getOverlayDiv().setToSizeOfWindow().show(); return this; },
            hide: function () { getOverlayDiv().hide(); return this; },
            fadeIn: function (callback) { getOverlayDiv().setToSizeOfWindow().fadeIn(function () { (callback || function () { })(); }); return this; },
            fadeOut: function (callback) { getOverlayDiv().fadeOut(function () { (callback || function () { })(); }); return this; }
        };
    })(),

    tooltip: (function () {
        var _tooltip;
        function getTooltipDiv() {
            if (!_tooltip)
                _tooltip = $('<div>').css({ position: 'absolute', bottom: '0', left: '0', 'z-index': '100', border: '1px solid #fdd', padding: '2px 5px', 'background-color': '#fee', 'font-size': '.9em', display: 'none' }).appendTo("body");
            return _tooltip;
        }

        return {
            show: function (x, y, contents) { getTooltipDiv().css({ left: x, bottom: $(window).height() - y }).html(contents).fadeIn(200); return this; },
            hide: function () { getTooltipDiv().hide(); return this; }
        };
    })(),

    post: function (url, data, callback) {
        $.post(url, $.extend({}, { '__RequestVerificationToken': $('[name="__RequestVerificationToken"]').val() }, data), function (returnedData) {
            if (typeof callback === "function") {
                callback(returnedData);
            }
        });
    },

    svc: {
        call: function (url, data, callback, async) {
            callback = callback || function () { };
            async = (async === null || async);

            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                url: url,
                data: JSON.stringify(data),
                dataType: 'json',
                async: async,
                success: function (jsonResult) {
                    if (jsonResult && typeof (jsonResult.d) !== 'undefined')
                        callback(jsonResult.d);
                    else
                        callback(jsonResult);
                },
                error: function () {
                    app.debug("Error calling '" + url + "' " + JSON.stringify(data));
                    callback({});
                }
            });

            return this;
        },
        callWithOverlay: function (url, data, callback, async) {
            app.overlay.show();
            return this.call(url, data, function (r) { callback(r); app.overlay.hide(); }, async);
        }
    },

    Delayed: function (callback, millisecondsToDelay) {
        /// <summary>
        ///     Use this to delay executing some code for specified period of time.
        ///     Useful in key events where you don't want events firing until there
        ///     is a pause in the typing.
        /// </summary>

        if (!(this instanceof app.Delayed)) throw 'call new';

        var timeout;

        this.execute = function () {
            window.clearTimeout(timeout);
            var _this = this;
            var args = arguments || [];
            timeout = window.setTimeout(function () { callback.apply(_this, args); }, millisecondsToDelay || 500);
        };

        this.cancel = function () {
            window.clearTimeout(timeout);
        };
    }

};

/******************************************************/
/******************************************************/

// Global initializations

$(function () {

    $('input[type=submit], button, .button').button();

});
