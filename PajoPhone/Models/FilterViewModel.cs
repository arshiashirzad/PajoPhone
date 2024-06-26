namespace PajoPhone.Models;

public enum FilterType
{
    Range,
    Dropdown,
    CheckBox
}
public class FilterViewModel
{
    public string Name { get; set; }
    public int FieldId { get; set; }
    public FilterType Type { get; set; }
    public string Value { get; set; }
}
