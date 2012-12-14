$(function () {

    var module = app.modules.homeIndex = {
        addInvoiceButton: $('#addInvoiceButton'),
        invoicesContainer: $('#invoicesContainer'),
        getInvoiceId: function(element) {
            return $(element).parents('article:first').data('invoiceid');
        }
    };

    app.overlay.show();
    module.invoicesContainer.load(app.webroot + 'invoices/load', function () {
        app.overlay.hide();
    });

    module.addInvoiceButton.click(function (e) {
        e.preventDefault();
        app.post(app.webroot + 'invoices/add', {}, function (result) {
            $(result)
                .hide()
                .prependTo(module.invoicesContainer)
                .slideDown('slow');
        });
    });

    $(document).on('click', 'a[href="#deleteInvoice"]', function (e) {
        e.preventDefault();

        var btn = $(this),
            invoice = module.getInvoiceId(btn),
            invoiceId = invoice.data('invoiceid');

        app.post(app.webroot + 'invoices/remove', { invoiceId: invoiceId });

        invoice.slideUp('slow', function () { invoice.remove(); });
    });

    $(document).on('click', 'a[href="#addLine"]', function(e) {
        e.preventDefault();

        var btn = $(this), invoiceId = module.getInvoiceId(btn);

        app.overlay.show();
        app.post(app.webroot + 'invoices/addlineitem', { invoiceId: invoiceId }, function (result) {
            btn.parents('table:first').find('tbody').html(result);
            app.overlay.hide();
        });
    });

    $(document).on('click', 'a[href="#printInvoice"]', function (e) {
        e.preventDefault();
        app.message('Coming soon');
    });

    $(document).on('click', '.editable.customer', function() {

        var container = $(this),
            invoiceId = module.getInvoiceId(container),
            dialog = $('<div>').hide().appendTo($('body'));

        function _onOk() {
            var data = $.extend({}, { invoiceId: invoiceId }, dialog.find('form').toObject());
            app.post(app.webroot + 'invoices/customereditform', data, function (result) {
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
            .load(app.webroot + 'invoices/customereditform?invoiceId=' + invoiceId, function() {
                dialog.find('[placeholder]').placeholder();
            });

    });

});
