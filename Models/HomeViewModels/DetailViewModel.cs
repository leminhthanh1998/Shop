using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models.HomeViewModels
{
    public class DetailViewModel
    {
        [HiddenInput]
        public int ItemsId { get; }
        public string Name { get; }
        public decimal Price { get; }
        public string Description { get; }
        public int QuantityOrdered { get; }
        public int VoteValue { get; set; }
        public List<string> getImagePathList { get; }
        public string SellerName { get; }
        public string SellerDescription { get; }
        public string FormatedAdress { get; }
        public string City { get; }
        [HiddenInput]
        public string Slug { get; }
        public string CategoryIcon { get; }
        public string CategoryName { get; }
        public string GetThumbPath { get; }
        public bool Approve { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "{0} phải có ít nhất {2} kí tự và tối đa {1} kí tự.", MinimumLength = 10)]
        public string CommentContent { get; set; }
        public Items Items;

        public DetailViewModel()
        {
        }

        public DetailViewModel(Items items)
        {
            ItemsId = items.ItemsId;
            Name = items.Name;
            Price = items.Price;
            Description = items.Description;
            QuantityOrdered = items.QuantityOrdered;
            getImagePathList = items.getImagePathList();
            SellerName = items.Seller.Name;
            SellerDescription = items.Seller.Description;
            CategoryIcon = items.Category.Icon;
            CategoryName = items.Category.Name;
            GetThumbPath = items.GetThumbPath();
            Slug = items.Slug;
            Approve = items.Approved;
        }
    }
}
