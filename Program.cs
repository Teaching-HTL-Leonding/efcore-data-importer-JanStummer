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


foreach (var customer in customers)
{
    await using (var transaction = await context.Database.BeginTransactionAsync())
    {
        try
        {
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            Console.WriteLine($"Transaction committed successfully for customer {customer.CompanyName}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error committing transaction for customer {customer.CompanyName}: {ex.InnerException!.Message}");
            Console.WriteLine($"Rolling back the transaction for customer {customer.CompanyName}");
        }
    }
}


