namespace PajoPhone.Models;

public class FilterViewModel
{
    public string Term { get; set; } = "";
    public int PageNo { get; set; }
    public int CategoryId { get; set; }
    public int MinPrice { get; set; } 
    public ICollection<FieldsValueViewModel> FieldsValueViewModels { get; set; } = new List<FieldsValueViewModel>();
}