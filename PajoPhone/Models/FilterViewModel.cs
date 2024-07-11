namespace PajoPhone.Models;

public class FilterViewModel
{
    public string Term { get; set; } = string.Empty;
    public int PageNo { get; set; } = 1;
    public int CategoryId { get; set; }
    public int MinPrice { get; set; }
    public List<FieldsValueViewModel> FieldsValueViewModels { get; set; } = [];
}   