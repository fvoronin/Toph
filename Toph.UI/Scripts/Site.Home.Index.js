(function (app, $, ko) {

    var vm = app.modules.homeIndex = {
        invoices: ko.observableArray(),
        noInvoicesMessage: ko.observable('Loading...'),
        addInvoice: addInvoice,
        deleteInvoice: deleteInvoice,
        printInvoice: printInvoice,
        addLine: addLine,
        editCustomer: editCustomer
    };

    ko.applyBindings(vm);
    setTimeout(init, 500);

    function init() {
        $.get(url('load'), function (invoices) {
            vm.invoices($.map(invoices, function (invoice) { return ko.mapping.fromJS(invoice); }));
            vm.noInvoicesMessage('No open invoices found');
        });
    }

    function addInvoice() {
        app.post(url('add'), {}, function (invoice) {
            vm.invoices.push(ko.mapping.fromJS(invoice));
            app.logger.info('Added invoice ' + invoice.InvoiceId);
        });
    }
    
    function deleteInvoice(invoice) {
        if (!confirm('Permanently delete this invoice?')) return;

        app.post(url('remove'), { invoiceId: invoice.InvoiceId() }, function() {
            vm.invoices.remove(invoice);
            app.logger.info('Deleted invoice ' + invoice.InvoiceId());
        });
    }
    
    function printInvoice(invoice) {
        app.logger.info('Coming soon (print invoice ' + invoice.InvoiceId() + ')');
    }

    function addLine(invoice) {
        app.post(url('addlineitem'), { invoiceId: invoice.InvoiceId() }, function (lineItem) {
            invoice.InvoiceLineItems.push(ko.mapping.fromJS(lineItem));
        });
    }

    function editCustomer(invoice) {

        var dialog = $('<div>').hide().appendTo($('body'));

        function _onOk() {
            var data = $.extend({}, { invoiceId: invoice.InvoiceId() }, dialog.find('form').toObject());
            app.post(url('customereditform'), data, function(result) {
                if ($(result).is('form')) {
                    dialog.html(result);
                } else {
                    dialog.dialog("close").remove();
                    ko.mapping.fromJS(result, {}, invoice.InvoiceCustomer);
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
            .load(url('customereditform?invoiceId=' + invoice.InvoiceId()));

    }

    function url(actionAndQuery) {
        return app.webroot + 'invoices/' + actionAndQuery;
    }

})(app, jQuery, ko);
