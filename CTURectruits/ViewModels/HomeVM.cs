using CTURectruits.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTURectruits.ViewModels
{
    public class HomeVM : ViewModel
    {
        public List<User> Users { get; set; }

        public User CurrentUserInstance { get; set; }

        public HomeVM()
        {
            this.CurrentUserInstance = CurrentUser;
        }


    }
}