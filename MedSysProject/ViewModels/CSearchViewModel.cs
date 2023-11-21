using MedSysProject.Models;

namespace MedSysProject.ViewModels
{
    public class CSearchViewModel
    {
        public List<Product>? products { get; set; }
        public List<ProductsClassification>? ProductsClass { get; set; }
        public List<ProductsCategory>? ProductsCategory { get; set; }
    }
}
