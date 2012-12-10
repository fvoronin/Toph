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
        app.post(app.webroot + 'invoices/add', {}, function(result) {
            module.invoicesContainer.prepend(result);
        });
    });

    $(document).on('click', '.addLine', function(e) {
        e.preventDefault();
        app.message('add line');
    });
});
