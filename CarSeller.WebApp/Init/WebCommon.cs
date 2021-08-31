using CarSeller.Common;
using CarSeller.Entities;
using CarSeller.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSeller.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            CarSellerUser user = CurrentSession.User;

            if (user != null)
                return user.Username;
            else
                return "system";
        }
    }
}