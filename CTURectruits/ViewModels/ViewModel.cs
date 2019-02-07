using CTURectruits.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTURectruits.ViewModels
{
    public class ViewModel
    {
        public static User CurrentUser { get; set; }
        
        private User currentUserInstance;
        public User CurrentUserInstance {
            get { return this.currentUserInstance; }
            set {
                this.currentUserInstance = value;
                CurrentUser = value;
            }
        }

        public ViewModel()
        {
            this.CurrentUserInstance = CurrentUser;
        }


    }
}