using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using dm106CarlosDrury.Models;
using dm106CarlosDrury.CRMClient;
using dm106CarlosDrury.br.com.correios.ws;
using System.Globalization;

namespace dm106CarlosDrury.Controllers
{
    [Authorize]
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        private dm106CarlosDruryContext db = new dm106CarlosDruryContext();

        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("frete")]
        public IHttpActionResult CalculaFrete(int id)
        {

            Order order = db.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }
            else
            {
                CRMRestClient crmClient = new CRMRestClient();
                Customer customer = crmClient.GetCustomerByEmail(order.emailUser);
                if (customer != null)
                {
                    // Initial Values
                    decimal larg = 0;
                    decimal comp = 0; 
                    decimal height = 0;
                    order.totalWeigth = 0;
                    order.totalPrice = 0;

                    // Calculate size for each orderItem
                    foreach (OrderItem orderItem in order.OrderItems)
                    {
                        int qtd = orderItem.qtd;
                        height = orderItem.Product.altura > height ? orderItem.Product.altura : height;
                        larg = orderItem.Product.largura > larg ? orderItem.Product.largura : larg;
                        comp += orderItem.Product.comprimento * qtd;
                        order.totalWeigth += orderItem.Product.peso * qtd;
                        order.totalPrice += orderItem.Product.preco * qtd;
                    }

                    // Correios API
                    string frete;
                    CalcPrecoPrazoWS correios = new CalcPrecoPrazoWS();
                    cResultado resultado = correios.CalcPrecoPrazo("", "", "40010", "37540000", customer.zip, order.totalWeigth.ToString(), 1, comp, height, larg, 0, "N", 100, "S");
                    if (resultado.Servicos[0].Erro.Equals("0"))
                    {
                        NumberFormatInfo numberFormat = new NumberFormatInfo();
                        numberFormat.NumberDecimalSeparator = ",";
                        order.shipmentPrice = Decimal.Parse(resultado.Servicos[0].Valor, numberFormat);
                        order.deliveryDate = order.orderDate.AddDays(Int32.Parse(resultado.Servicos[0].PrazoEntrega));
                        frete = "Valor do frete: " + order.shipmentPrice + " - Prazo de entrega: " + order.deliveryDate;
                        db.SaveChanges();

                        return Ok(frete);
                    }
                    else
                    {
                        return BadRequest("Código do erro: " + resultado.Servicos[0].Erro + "-" + resultado.Servicos[0].MsgErro);
                    }
                }
                else
                {
                    return BadRequest("Falha ao consultar o CRM");
                }
            }
                    
        }

           
        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("cep")]
        public IHttpActionResult obtemCEP()
        {
            CRMRestClient crmClient = new CRMRestClient();
            Customer customer = crmClient.GetCustomerByEmail(User.Identity.Name);

            if (customer != null)
            {
                return Ok(customer.zip);
            }
            else
            {
                return BadRequest("Falha ao consultar o CRM");
            }
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        [HttpGet]
        [Route("close")]
        public IHttpActionResult CloseOrder(int id)
        {
            Order order = db.Orders.Find(id);

            if (order == null)
                return NotFound();

            if (order.emailUser == User.Identity.Name || User.IsInRole("ADMIN"))
            {
                // order found
                if (order.shipmentPrice != 0)
                {
                    order.status = "fechado";
                    db.SaveChanges();
                    return Ok("Order Closed");
                }
                else
                {
                    return BadRequest("Cannot close order due to shipment price was not defined.");
                }
            }             
            else 
            {
                return BadRequest("Authorization Denied! Only admin or the order owner allowed!");
            }
           
                
        }

        // GET: api/Orders
        public List<Order> GetOrders()
        {
            if (User.IsInRole("ADMIN"))
            {
                return db.Orders.Include(order => order.OrderItems).ToList();
            }
            else
            {
                return db.Orders.Where(order => order.emailUser == User.Identity.Name).ToList();
            }
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order != null && (order.emailUser == User.Identity.Name || User.IsInRole("ADMIN")))
                return Ok(order);
            else
                return NotFound();
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if(order != null && (order.emailUser == User.Identity.Name || User.IsInRole("ADMIN")))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != order.Id)
                {
                    return BadRequest();
                }

                db.Entry(order).State = EntityState.Modified;

                try
                {
                    if (order != null && (order.emailUser == User.Identity.Name || User.IsInRole("ADMIN")))
                    {
                        db.SaveChanges();
                    }
                    else
                    {
                        return BadRequest();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return BadRequest("Authorization Denied! Only admin or the order owner allowed!");
            }
                

            
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {

            // initialize order
            order.status = "novo";
            order.totalWeigth = 0;
            order.shipmentPrice = 0;
            order.totalPrice = 0;
            order.orderDate = DateTime.Now;
            order.emailUser = User.Identity.Name;


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(order);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);

            if (order == null)
                return NotFound();

            if (order.emailUser == User.Identity.Name || User.IsInRole("ADMIN"))
            {
                db.Orders.Remove(order);
                db.SaveChanges();

                return Ok(order);
            }
            else
            {
                return BadRequest("Authorization Denied! Only admin or the order owner allowed!");
            }


        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}