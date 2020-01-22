using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Items
    {
        [JsonProperty]
        public int ItemsId { get; set; }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mặt hàng nên có một cái tên");
                if (value.Length > 30)
                    throw new ArgumentException("Tên tối đa 30 ký tự.");
                _name = value;
            }
        }

        private decimal _price;

        public decimal Price {
            get { return _price; }
            set
            {
                if (value < 1 || value > 100000000)
                    throw new ArgumentException("Giá của mặt hàng phải nằm trong khoảng từ 1 vnđ đến 100000000 vnđ");
                _price = value;
            }
        }
        [JsonProperty]
        public string Description { get; set; }
        [JsonProperty]
        public int QuantityOrdered { get; set; }
        [JsonProperty]
        public string Image { get; set; }
        public string Slug { get; set; }
        public Seller Seller { get; set; }

        public Category _category;
        public Category Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value ?? throw new ArgumentException("Thể loại * bắt buộc.");
            }
        }
      
        public bool Approved { get; set; }
        public Offer Offer { get; set; }

        public virtual IEnumerable<Comment> Comments { get; set; }
        public virtual IEnumerable<Vote> Votes { get; set; }

        [JsonConstructor]
        protected Items() { }

        public Items(string name, decimal price, string description, int quantityOrdered, string image, Category category, Seller seller, Offer offer, string slug, bool approved = false    )
        {
            Name = name;
            Approved = approved;
            Price = price;
            Description = description;
            QuantityOrdered = quantityOrdered;
            Image = image;
            Category = category;
            Seller = seller;
            Offer = offer;
            Slug = slug;
        }

        
        public String GetThumbPath()
        {
            string path = Image + "thumb.jpg";
            return path;
        }

        public List<string> getImagePathList()
        {
            return Directory.GetFiles(@"wwwroot\" + Image + @"Image\", "*.JPG").Select(l => l.Replace(@"wwwroot\", "")).ToList();
        }
    }
}
