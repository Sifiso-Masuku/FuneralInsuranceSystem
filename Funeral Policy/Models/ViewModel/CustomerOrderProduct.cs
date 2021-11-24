//using OrderMgmtUsingEF.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Funeral_Policy.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Funeral_Policy.ViewModel
{
    public class CustomerOrderProduct
    {
        [Key]
        public int CustomerOrderProductID { get; set; }
        [DisplayName("Customer Email")]
        public string CustomerEmail { get; set; }
        [DisplayName("Order No")]
        public int OrderID { get; set; }
        [DisplayName("Name")]
        public string  CustomerName { get; set; }
        [DisplayName("Status")]
        public string OrderStatus { get; set; }
        public double funeralprice { get; set; }
        public string Coffin { get; set; }
        public string funeralname { get; set; }
        public int FuneralType { get; set; }
        public double CoffinPrice { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "datetime2")]
        [Required, DisplayName("Funeral Date")]
        public DateTime funeralDate { get; set; }
        public DateTime Time { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "datetime2")]
        public DateTime EndDate { get; set; }
        public OrderProduct order { get; set; }
        public List<OrderProduct> orderProducts { get; set; }
        [DataType(DataType.Currency)]

        [DisplayName("Total Amount")]
        public decimal TotalAmount { get; set; }
        public FuneralBooking or { get; set; }
        public List<FuneralBooking> funeralBookings { get; set; }
    }
    public class OrderProduct
    {
        
        [DisplayName("Item Code")]
        public int ProductID { get; set; }
        [DisplayName("Item Name")]
        public string ProductName { get; set; }
       
        [DisplayName("Price")]
        public double UnitPrice { get; set; }
        [DisplayName("Quantity")]
        public int dty { get; set; }
        [DisplayName("Sub-Total")]
        public double TotalPrice { get; set; }
    }

}