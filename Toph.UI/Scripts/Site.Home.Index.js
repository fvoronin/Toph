$(function () {

    var module = app.modules.homeIndex = {
        addInvoiceButton: $('#addInvoiceButton'),
        invoicesContainer: $('#invoicesContainer')
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
            invoice = btn.parents('article:first'),
            invoiceId = invoice.data('invoiceid');

        app.post(app.webroot + 'invoices/remove', { invoiceId: invoiceId });

        invoice.slideUp('slow', function () { invoice.remove(); });
    });

    $(document).on('click', 'a[href="#addLine"]', function(e) {
        e.preventDefault();

        var btn = $(this), invoiceId = btn.parents('article:first').data('invoiceid');

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

});
