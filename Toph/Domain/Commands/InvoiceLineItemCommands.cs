using System;
using System.ComponentModel.DataAnnotations;
using Toph.Common.DataAccess;
using Toph.Domain.Entities;

namespace Toph.Domain.Commands
{
    public class EditInvoiceLineItemCommand : ICommand
    {
        public EditInvoiceLineItemCommand()
        {
        }

        public EditInvoiceLineItemCommand(InvoiceLineItem item)
        {
            Id = item.Id;
            Version = item.Version;
            Date = item.LineItemDate.ToString("MM/dd/yyyy");
            Description = item.Description;
            Quantity = item.Quantity;
            Price = item.Price;
        }

        public int Id { get; set; }
        public int Version { get; set; }

        [Required, DataType(DataType.Date)]
        public string Date { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0d, double.MaxValue)]
        public double Quantity { get; set; }

        [Range(0d, double.MaxValue)]
        public double Price { get; set; }
    }

    public class InvoiceLineItemCommandHandler : ICommandHandler<EditInvoiceLineItemCommand>
    {
        public InvoiceLineItemCommandHandler(IRepository repository, IValidationFacade validationFacade)
        {
            _repository = repository;
            _validationFacade = validationFacade;
        }

        private readonly IRepository _repository;
        private readonly IValidationFacade _validationFacade;

        public CommandResult Handle(EditInvoiceLineItemCommand command)
        {
            var result = _validationFacade.Validate(command);
            if (result.AnyErrors())
                return result;

            var item = _repository.Get<InvoiceLineItem>(command.Id, command.Version);

            item.LineItemDate = DateTimeOffset.Parse(command.Date);
            item.Description = command.Description ?? "";
            item.Quantity = command.Quantity;
            item.Price = command.Price;

            return result;
        }
    }
}
