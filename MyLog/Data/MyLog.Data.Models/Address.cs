namespace MyLog.Data.Models;

public class Address
{
    public Address(string street, string city, string postCode, string name)
    {
        Street = street;
        City = city;
        PostCode = postCode;
        Name = name;
    }

    public string Street { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
    public int Id { get; set; } = 0;
    public string Name { get; set; }
}
