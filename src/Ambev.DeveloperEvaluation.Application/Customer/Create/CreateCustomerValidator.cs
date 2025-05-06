namespace Ambev.DeveloperEvaluation.Application.Customer.Create;

public class CreateCustomerValidator
{
    public static void Validate(CreateCustomerCommand command)
    {
        if (string.IsNullOrEmpty(command.CustomerName))
            throw new InvalidOperationException("Customer name is required");
    }
}

