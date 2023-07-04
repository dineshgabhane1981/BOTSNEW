using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DLC.ViewModel
{
    public class UpdateProfileViewModel
    {
        public List<tblDLCProfileUpdateConfig_Publish> lstProfileData { get; set; }
        public string dummyGender { get; set; }
        public string dummyMaritalStatus { get; set; }
        public SelectListItem[] Gender()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Male", Value = "M" }, new SelectListItem() { Text = "Female", Value = "F" } };
        }
        public SelectListItem[] MaritalStatus()
        {
            return new SelectListItem[4] { new SelectListItem() { Text = "Married", Value = "M" }, new SelectListItem() { Text = "Unmarried", Value = "U" }, new SelectListItem() { Text = "Single", Value = "S" }, new SelectListItem() { Text = "Divorced", Value = "D" } };
        }
    }
}