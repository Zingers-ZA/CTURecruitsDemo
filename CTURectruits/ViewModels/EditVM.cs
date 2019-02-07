using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CTURectruits.Models;
namespace CTURectruits.ViewModels
{
    public class EditVM : ViewModel
    {
        public User userToEdit { get; set; }

        public User editedUser { get; set; }

    } 
}