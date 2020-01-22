using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Shop.Models.Domain;
using Shop.Models.Domain.Enum;
using Shop.Models.ManageViewModels;

namespace Shop.Models.AdminViewModels
{
    public class SoldItemsViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Id hóa đơn")]
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Ngày tạo")]
        public string CreationDate { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Tên mặt hàng")]
        public string ItemsName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Ngày sử dụng")]
        public string UseDate { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Hiệu lực")]
        public string Status { get; set; }

        public string StatusClass { get; }

        [DataType(DataType.Text)]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Tên")]
        public string StationName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Địa chỉ khách hàng")]
        public string Address { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Tên khách hàng")]
        public string NameReciever { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Email người nhận")]
        public string EmailReciever { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Email khách hàng")]
        public string EmailSender { get; set; }

        public IEnumerable<DetailOrderModel> DetailOrders { get; set; }



        public SoldItemsViewModel(OrderItem order, IEnumerable<DetailOrderModel> detailOrders, int id)
        {
            DetailOrders = detailOrders;
            Id = id ;
            Name = order.Seller.Name;
            ItemsName = order.Items.Name;
            Price = order.Price;
            CreationDate = order.CreationDate.ToString("dd/MM/yyyy");
            Status = order.Validity.ToString();
            UseDate = order.ExpirationDate.ToString("dd/MM/yyyy");
            Address = order.SenderAddress;
            StationName = order.SenderName;
            NameReciever = order.RecipientName;
            EmailReciever = order.RecipientEmail;
            EmailSender = order.SenderEmail;

            switch (order.Validity)
            {
                case Validity.Used:
                    StatusClass = "label-success";
                    break;
                case Validity.Valid:
                    StatusClass = "label-primary";
                    break;
                case Validity.Expired:
                    StatusClass = "label-danger";
                    break;
                default:
                    StatusClass = "label-primary";
                    break;
            }

        }

        public SoldItemsViewModel()
        {

        }

    }
}
