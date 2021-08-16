using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModel
{
    public class CustomerDashboardViewModel
    {
        public List<CustomerListing> customerListings { get; set; }
        public List<OnBoardingListing> onBoardingListings { get; set; }
    }
}