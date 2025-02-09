﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models.Domain;

namespace Shop.Models.AdminViewModels
{
    public class SellerRequestListViewModel
    {
        public int Id { get; }
        public string City { get; }
        public string Postcode { get; }
        public string Name { get; }

        public SellerRequestListViewModel(Seller seller)
        {
            Id = seller.SellerId;
            City = seller.City;
            Name = seller.Name;
            Postcode = seller.Postcode;
        }

    }
}
