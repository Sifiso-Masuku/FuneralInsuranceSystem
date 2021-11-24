using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Funeral_Policy.Models.CartModels;
using Funeral_Policy.Services.CartServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Funeral_Policy.Models;

namespace Funeral_Policy.Controllers.CartController
{
    public class ShoppingController : Controller
    {
        private Cart_Service cart_Service;
        private Item_Service item_Service;
        private Order_Service order_Service;
        private Address_Service address_Service;
        Category_Service Category_Service;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ShoppingController()
        {
            this.cart_Service = new Cart_Service();
            this.item_Service = new Item_Service();
            this.order_Service = new Order_Service();
            this.address_Service = new Address_Service();
            this.Category_Service = new Category_Service();
        }
        public ActionResult Index(int? id)
        {
            var items_results = new List<Item>();
            try
            {
                if (id != null)
                {
                    if (id == 0)
                    {
                        items_results = item_Service.GetItems();
                        ViewBag.Department = "All Categories";
                    }
                    else
                    {
                        items_results = item_Service.GetItems().Where(x => x.Category_ID == (int)id).ToList();
                        ViewBag.Department = Category_Service.GetCategory(id).Name;
                    }
                }
                else
                {
                    items_results = item_Service.GetItems();
                    ViewBag.Department = "All Categories";
                }
            }
            catch (Exception ex) { }
            return View(items_results);
        }
        public ActionResult add_to_cart(int id)
        {
            var UserName = User.Identity.GetUserName();
            int qty = 1;
            var item = item_Service.GetItem(id);

            Cart_Item ct = new Cart_Item();

            if (item != null)
            {
                cart_Service.UpdateQuantity(id, qty);
                cart_Service.AddItemToCart(id, UserName);
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult remove_from_cart(string id)
        {
            var item = cart_Service.GetCartItems().FirstOrDefault(x => x.cart_item_id == id);
            if (item != null)
            {

                cart_Service.RemoveItemFromCart(id: id);
                return RedirectToAction("ShoppingCart");
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult ShoppingCart()
        {
            ViewBag.Total = cart_Service.GetCartTotal(cart_Service.GetCartID());
            ViewBag.TotalQTY = cart_Service.GetCartItems().FindAll(x => x.cart_id == cart_Service.GetCartID()).Sum(q => q.quantity);
            return View(cart_Service.GetCartItems().FindAll(x => x.cart_id == cart_Service.GetCartID()));
        }
        [HttpPost]
        public ActionResult ShoppingCart(List<Cart_Item> items)
        {
            foreach (var i in items)
            {
                cart_Service.UpdateCart(i.cart_item_id, i.quantity);
            }
            return RedirectToAction("ShoppingCart");
        }
        public ActionResult countCartItems()
        {
            var username = User.Identity.GetUserName();
            int qty = cart_Service.GetCartItems().Sum(x => x.quantity);
            return Content(qty.ToString());
        }
        public ActionResult Checkout()
        {
            FuneralBooking c = new FuneralBooking();
            if (cart_Service.GetCartItems().Count == 0)
            {
                ViewBag.Err = "Opps... you should have atleat one cart item, please shop a few items";
                return RedirectToAction("Index");
            }
            else
                cart_Service.EmptyCart();
            cart_Service.checkout(User.Identity.Name);
            return RedirectToAction("Index", "FuneralBookings");

        }
        [Authorize]
        public ActionResult HowToGetMyOrder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult HowToGetMyOrder(string street_number, string street_name, string City, string State, string ZipCode, string Country)
        {
            Session["street_number"] = street_number;
            Session["street_name"] = street_name;
            Session["City"] = City;
            Session["State"] = State;
            Session["ZipCode"] = ZipCode;
            Session["Country"] = Country;
            return RedirectToAction("PlaceOrder", new { id = "deliver" });
        }

    }
}