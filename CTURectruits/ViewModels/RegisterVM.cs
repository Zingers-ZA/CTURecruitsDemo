using CTURectruits.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTURectruits.ViewModels
{
    public class RegisterVM : ViewModel
    {
        public User tempUser { get; set; }

        public User createdUser { get; set; } 

    }
}