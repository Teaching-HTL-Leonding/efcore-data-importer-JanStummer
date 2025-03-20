using System.Runtime.CompilerServices;
using EFCoreWriting;
using Microsoft.EntityFrameworkCore;

var factory = new ApplicationDataContextFactory();
var context = factory.CreateDbContext(args: []);

await context.OrderLines.ExecuteDeleteAsync();
await context.OrderHeaders.ExecuteDeleteAsync();
await context.Customers.ExecuteDeleteAsync();

var input = await File.ReadAllLinesAsync("/Users/13san/Desktop/Prog4AHIF/efcore-data-importer-JanStummer/data.txt");

static List<Customer> ParseInput(string[] input)
    {
        List<Customer> customers = new List<Customer>();
        Customer? currentCustomer = null;
        OrderHeader? currentOrderHeader = null;

        foreach (var line in input)
        {
            string[] parts = line.Split('|');
            switch (parts[0])
            {
                case "CUS":
                    currentCustomer = new Customer(0, parts[1], parts[2], parts[3])
                    {
                        Orders = new List<OrderHeader>()
                    };
                    customers.Add(currentCustomer);
                    break;
                case "OH":
                    currentOrderHeader = new OrderHeader(0, 0, currentCustomer, DateOnly.Parse(parts[1]), parts[2], parts[3], parts[4])
                    {
                        OrderLines = new List<OrderLine>()
                    };
                    currentCustomer.Orders.Add(currentOrderHeader);
                    break;
                case "OL":
                    var orderLine = new OrderLine(0, 0, currentOrderHeader, parts[1], int.Parse(parts[2]), decimal.Parse(parts[3]));
                    currentOrderHeader.OrderLines.Add(orderLine);
                    break;
            }
        }

        return customers;
    }


var customers = ParseInput(input);


await using var transaction = await context.Database.BeginTransactionAsync();
try
{
    foreach (var customer in customers)
    {
        if (customer.CountryIsoCode.Length != 2)
        {
            Console.Error.WriteLine($"Skipping customer '{customer.CompanyName}' due to invalid CountryIsoCode '{customer.CountryIsoCode}'");
            continue;
        }

        context.Customers.Add(customer);
    }

    await context.SaveChangesAsync();
    await transaction.CommitAsync();
    Console.WriteLine("Transaction committed successfully for all customers.");
}
catch (DbUpdateException ex)
{
    await transaction.RollbackAsync();
    Console.Error.WriteLine($"Database error: {ex.InnerException?.Message ?? ex.Message}. Rolling back transaction.");
}
catch (Exception ex)
{
    await transaction.RollbackAsync();
    Console.Error.WriteLine($"Unexpected error: {ex.Message}. Rolling back transaction.");
}



