(function (app, $, ko) {

    var module = app.modules.homeIndex = {
        vm: {
            noInvoicesMessage: ko.observable('Loading...'),
            invoices: ko.observableArray(),
            addInvoice: addInvoice,
            deleteInvoice: deleteInvoice,
            printInvoice: printInvoice,
            addLine: addLine
        },
        invoicesContainer: $('#invoicesContainer'),
        url: function(actionAndQuery) {
            return app.webroot + 'invoices/' + actionAndQuery;
        },
        getInvoice: function(element) {
            return $(element).parents('article:first');
        },
        getInvoiceId: function(element) {
            return module.getInvoice(element).data('invoiceid');
        }
    };

    ko.applyBindings(module.vm);
    setTimeout(init, 100);

    function init() {
        $.get(module.url('load'), function (invoices) {
            module.vm.invoices($.map(invoices, function (invoice) { return ko.mapping.fromJS(invoice); }));
            module.vm.noInvoicesMessage('No open invoices found');
        });
    }

    function addInvoice() {
        app.post(module.url('add'), {}, function (invoice) {
            module.vm.invoices.push(ko.mapping.fromJS(invoice));
            app.logger.info('Added invoice ' + invoice.InvoiceId);
        });
    }
    
    function deleteInvoice(invoice) {
        app.post(module.url('remove'), { invoiceId: invoice.InvoiceId() }, function() {
            module.vm.invoices.remove(invoice);
            app.logger.info('Deleted invoice ' + invoice.InvoiceId());
        });
    }
    
    function printInvoice(invoice) {
        app.logger.info('Coming soon (print invoice ' + invoice.InvoiceId() + ')');
    }

    function addLine(invoice) {
        app.post(module.url('addlineitem'), { invoiceId: invoice.InvoiceId() }, function (lineItem) {
            invoice.InvoiceLineItems.push(ko.mapping.fromJS(lineItem));
        });
    }

})(app, jQuery, ko);
