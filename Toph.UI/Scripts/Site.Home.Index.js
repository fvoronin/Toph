$(function () {

    var module = app.modules.homeIndex = {
        addInvoiceButton: $('#addInvoiceButton'),
        invoicesContainer: $('#invoicesContainer'),
        url: function(actionAndQuery) {
            return app.webroot + 'invoices/' + actionAndQuery;
        },
        getInvoice: function (element) {
            return $(element).parents('article:first');
        },
        getInvoiceId: function (element) {
            return module.getInvoice(element).data('invoiceid');
        }
    };

    app.overlay.show();
    module.invoicesContainer.load(module.url('load'), function () {
        app.overlay.hide();
    });

    module.addInvoiceButton.click(function (e) {
        e.preventDefault();
        app.post(module.url('add'), {}, function (result) {
            $(result)
                .hide()
                .prependTo(module.invoicesContainer)
                .slideDown('slow');
        });
    });

    module.invoicesContainer.on('click', 'a[href="#deleteInvoice"]', function (e) {
        e.preventDefault();

        var btn = $(this),
            invoice = module.getInvoice(btn),
            invoiceId = module.getInvoiceId(btn);

        app.post(module.url('remove'), { invoiceId: invoiceId });

        invoice.slideUp('slow', function () { invoice.remove(); });
    });

    module.invoicesContainer.on('click', 'a[href="#addLine"]', function (e) {
        e.preventDefault();

        var btn = $(this), invoiceId = module.getInvoiceId(btn);

        app.overlay.show();
        app.post(module.url('addlineitem'), { invoiceId: invoiceId }, function (result) {
            btn.parents('table:first').find('tbody').html(result);
            app.overlay.hide();
        });
    });

    module.invoicesContainer.on('click', 'a[href="#printInvoice"]', function (e) {
        e.preventDefault();
        app.message('Coming soon');
    });

    module.invoicesContainer.on('click', '.editable.customer', function () {

        var container = $(this),
            invoiceId = module.getInvoiceId(container),
            dialog = $('<div>').hide().appendTo($('body'));

        function _onOk() {
            var data = $.extend({}, { invoiceId: invoiceId }, dialog.find('form').toObject());
            app.post(module.url('customereditform'), data, function (result) {
                if ($(result).is('form')) {
                    dialog.html(result);
                } else {
                    dialog.dialog("close").remove();
                    container.html(result);
                }
            });
        }

        dialog
            .dialog(
                {
                    title: 'Customer Information',
                    modal: true,
                    width: 'auto',
                    position: 'top+10%',
                    buttons: [
                        { 'text': 'OK', click: _onOk },
                        { 'text': 'Cancel', click: function() { dialog.dialog("close").remove(); } }
                    ]
                })
            .append($('<p>').text('Loading...'))
            .load(module.url('customereditform?invoiceId=' + invoiceId));

    });

});
