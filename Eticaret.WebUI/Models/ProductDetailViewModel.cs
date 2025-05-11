using Eticaret.Core.Entities;

namespace Eticaret.WebUI.Models
{

    //Ürün detayında ilişkili ürünleri gösterme
    public class ProductDetailViewModel
    {
        public Product? Product { get; set; }
        public IEnumerable<Product>? RelatedProducts { get; set; } //sadece gösterim amaçlı olduğundan List değil IEnumerable kullandım 
    }
}
