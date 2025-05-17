using Microsoft.AspNetCore.Mvc;

namespace VegEaseBackend.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Products/Profile.cshtml");
        }

        public IActionResult LoadContent(string section)
        {
            switch (section)
            {
                case "profile":
                    return PartialView("~/Views/Profile_items/Profile.cshtml");
                case "wallet":
                    return PartialView("~/Views/Profile_items/Wallet.cshtml");
               
                case "rewards":
                    return PartialView("~/Views/Profile_items/Rewards.cshtml");
                case "refer-earn":
                    return PartialView("~/Views/Profile_items/ReferAndEarn.cshtml");
   
                case "address":
                    return PartialView("~/Views/Profile_items/Address.cshtml");
                case "orders":
                    return PartialView("~/Views/Profile_items/Orders.cshtml");
                case "returns":
                    return PartialView("~/Views/Profile_items/ReturnRequests.cshtml");
                case "notification":
                    return PartialView("~/Views/Profile_items/Notification.cshtml");
                case "admin":
                    return PartialView("~/Views/Profile_items/Admin.cshtml");
                default:
                    return PartialView("~/Views/Profile_items/Profile.cshtml");
            }
        }
    }
}
