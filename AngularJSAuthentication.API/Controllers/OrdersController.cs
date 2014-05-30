using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            var userName = principal.Claims.Where(c => c.Type == "sub").Single().Value;

            return Ok(Order.CreateOrders(userName));
        }

    }


    #region Helpers

    public class Order
    {
        public int OrderID { get; set; }
        public string OrderFor { get; set; }
        public string CustomerName { get; set; }
        public string ShipperCity { get; set; }
        public Boolean IsShipped { get; set; }


        public static List<Order> CreateOrders(string orderFor)
        {
            List<Order> OrderList = new List<Order> 
            {
                new Order {OrderID = 10248, OrderFor = orderFor, CustomerName = "Taiseer Joudeh", ShipperCity = "Amman", IsShipped = true },
                new Order {OrderID = 10249, OrderFor = orderFor, CustomerName = "Ahmad Hasan", ShipperCity = "Dubai", IsShipped = false},
                new Order {OrderID = 10250, OrderFor = orderFor,CustomerName = "Tamer Yaser", ShipperCity = "Jeddah", IsShipped = false },
                new Order {OrderID = 10251, OrderFor = orderFor,CustomerName = "Lina Majed", ShipperCity = "Abu Dhabi", IsShipped = false},
                new Order {OrderID = 10252, OrderFor = orderFor,CustomerName = "Yasmeen Rami", ShipperCity = "Kuwait", IsShipped = true}
            };

            return OrderList;
        }
    }

    #endregion
}
