namespace PajoPhone.Models;

public class FieldsValueViewModel
{
    public int ValueId { get; set; }
    public int KeyId { get; set; }
    public string? Key { get; set; }
    public string? StringValue { get; set; }
    public int IntValue { get; set; }

    public FieldsValueViewModel(){}
    public FieldsValueViewModel(FieldsKey fieldsKey)
    {
        Key = fieldsKey.Key;
        KeyId = fieldsKey.Id;
    }
    public FieldsValueViewModel(FieldsValue fieldsValue)
    {
        StringValue = fieldsValue.StringValue;
        IntValue = fieldsValue.IntValue;
        ValueId = fieldsValue.Id;
        KeyId = fieldsValue.FieldKeyId;
        Key =fieldsValue.FieldKey.Key;
    }
}