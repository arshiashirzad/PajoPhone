namespace PajoPhone.Models;

public class FieldsValue
{
    public int Id { get; set; }
    public string StringValue { get; set; }
    public int IntValue { get; set; }
    
    public int FieldKeyId { get; set; }
    public FieldsKey FieldKey { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }
}