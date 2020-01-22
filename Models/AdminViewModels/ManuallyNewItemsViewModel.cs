using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class ManuallyNewItemsViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Mã người bán hàng")]
        public int SellerId { get; set; } = 1;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }


        [Required]
        [Display(Name = "Hãng điện thoại")]
        public string Category { get; set; }

        

        [Required]
        [Display(Name = "Offer")]
        public Offer Offer { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Hình ảnh thu nhỏ")]
        public IFormFile Thumbnail { set; get; }

        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Hình ảnh")]
        public List<IFormFile> Image { set; get; }


    }
}
