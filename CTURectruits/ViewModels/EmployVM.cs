using CTURectruits.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CTURectruits.ViewModels
{
    public class EmployVM : ViewModel
    {
        public List<User> Users         { get; set; }
        public string     keywordFilter { get; set; }
        public int        yearsFilter   { get; set; }
    }
}