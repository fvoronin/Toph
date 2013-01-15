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

    logger: (function () {
        if (!toastr) throw 'toastr plugin not referenced';

        toastr.options.timeOut = 2000;
        toastr.options.positionClass = 'toast-bottom-right';

        return {
            error: function (message, title) {
                toastr.error(message, title);
                log('Error: ' + message);
            },
            info: function (message, title) {
                toastr.info(message, title);
                log('Info: ' + message);
            },
            success: function (message, title) {
                toastr.success(message, title);
                log('Success: ' + message);
            },
            warning: function (message, title) {
                toastr.warning(message, title);
                log('Warning: ' + message);
            },
            logonly: log
        };

        function log() {
            var console = window.console;
            !!console && console.log && console.log.apply && console.log.apply(console, arguments);
        }

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
        $.post(url, $.extend({ '__RequestVerificationToken': $('[name="__RequestVerificationToken"]').val() }, data), function (result) {
            if (typeof callback === "function") {
                callback(result);
            }
        });
    },

    createDialogForm: function(loadUrl, callback) {
        var _dialog = $('<div>').hide().appendTo($('body'));

        _dialog.on('submit', 'form', function(e) {
            var form = $(this);

            e.preventDefault();

            $.validator.unobtrusive.parse(form);
            if (!form.valid()) {
                reposition();
                return false;
            }

            app.post(form.attr('action'), form.toObject(), function(result) {
                if (typeof result == 'string') {
                    _dialog.html(result);
                    reposition();
                } else {
                    _dialog.dialog('close');
                    callback(result);
                }
            });

            return false;
        });

        _dialog
            .dialog({ modal: true, width: 'auto', close: function () { _dialog.remove(); } })
            .append($('<p>').text('Loading...'))
            .load(loadUrl, reposition);

        function reposition() {
            // forces it to re-position considering any new content
            _dialog.dialog('option', 'position', _dialog.dialog('option', 'position'));
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
