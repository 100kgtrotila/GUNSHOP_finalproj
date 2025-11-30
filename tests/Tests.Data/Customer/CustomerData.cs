using Domain.Customers;

namespace Tests.Data.Customer;

public static class CustomerData
{
    public static Domain.Customers.Customer FirstCustomer() => 
        Domain.Customers.Customer.New(
            "John", 
            "Doe", 
            "john.doe@example.com", 
            "+380123456789", 
            "LIC-12345");

    public static Domain.Customers.Customer SecondCustomer() => 
        Domain.Customers.Customer.New(
            "Jane", 
            "Smith", 
            "jane.smith@example.com", 
            "+380987654321", 
            "LIC-67890");

    public static Domain.Customers.Customer UnverifiedCustomer() => 
        Domain.Customers.Customer.New(
            "Bob", 
            "Johnson", 
            "bob.johnson@example.com", 
            "+380555555555", 
            "LIC-11111");
    
    public static Domain.Customers.Customer ThirdCustomer() => 
        Domain.Customers.Customer.New(
            "Alice", 
            "Wonder", 
            "alice.wonder@test.com", 
            "+380501111111", 
            "LIC-NEW-2024");
}