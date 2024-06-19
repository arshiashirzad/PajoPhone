namespace PajoPhone.Models;

public class FieldsValue
{
    public int Id { get; set; }
    public string Value { get; set; }

    public int FieldId { get; set; }
    public virtual Fields Field { get; set; }

    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
}