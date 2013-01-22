(function (app, $, ko) {

    var vm = app.modules.homeIndex = {
        invoices: ko.observableArray(),
        noInvoicesMessage: ko.observable('Loading...'),
        onAddInvoiceClick: onAddInvoiceClick,
        onDeleteInvoiceClick: onDeleteInvoiceClick,
        onExportInvoiceClick: onExportInvoiceClick,
        onAddLineItemClick: onAddLineItemClick,
        onCustomerClick: onCustomerClick,
        onLineItemClick: onLineItemClick
    };

    $(function() {
        ko.applyBindings(vm);

        $.get(url(), function (invoices) {
            vm.invoices($.map(invoices, extendInvoice));
            vm.noInvoicesMessage('No open invoices found');
        });
    });

    function onAddInvoiceClick() {
        app.post(url('add'), {}, function (invoice) {
            vm.invoices.push(extendInvoice(invoice));
            app.logger.info('Added invoice ' + invoice.InvoiceId);
        });
    }
    
    function onDeleteInvoiceClick(invoice) {
        if (!confirm('Permanently delete this invoice?')) return;

        app.post(url('remove'), { invoiceId: invoice.InvoiceId() }, function() {
            vm.invoices.remove(invoice);
            app.logger.info('Deleted invoice ' + invoice.InvoiceId());
        });
    }
    
    function onExportInvoiceClick(invoice) {
        app.logger.info('Coming soon (print invoice ' + invoice.InvoiceId() + ')');
    }

    function onAddLineItemClick(invoice) {
        app.post(url('addlineitem'), { invoiceId: invoice.InvoiceId() }, function (lineItem) {
            invoice.InvoiceLineItems.push(ko.mapping.fromJS(lineItem));
        });
    }

    function onCustomerClick(invoice) {
        app.createDialogForm(url('customereditform?invoiceId=' + invoice.InvoiceId()), function(result) {
            ko.mapping.fromJS(result, {}, invoice.InvoiceCustomer);
        });
    }

    function onLineItemClick(lineItem) {
        var dialog = app.createDialogForm(url('LineItemEditForm/' + lineItem.LineItemId()), function(result) {
            ko.mapping.fromJS(result, {}, lineItem);
        }).on('delete', function (e) {
            e.preventDefault();

            dialog.dialog('close');
            lineItem.Invoice.InvoiceLineItems.remove(lineItem);
            app.post(url('deletelineitem'), { id: lineItem.LineItemId() });

            return false;
        });
    }


    function extendInvoice(invoice) {
        invoice = ko.mapping.fromJS(invoice);

        invoice.InvoiceTotal = ko.computed(function() {
            var total = 0.0;
            $.each(invoice.InvoiceLineItems(), function () {
                total += parseFloat(this.LineItemTotal().replace(/[^-.\d]/g, ''));
            });
            return total;
        });

        $.each(invoice.InvoiceLineItems(), function (index, lineItem) {
            lineItem.Invoice = invoice;
        });

        return invoice;
    }

    function url(actionAndQuery) {
        var result = app.webroot + 'invoices';
        if (actionAndQuery && actionAndQuery.length > 0)
            result += '/' + actionAndQuery;
        return result;
    }

})(app, jQuery, ko);
