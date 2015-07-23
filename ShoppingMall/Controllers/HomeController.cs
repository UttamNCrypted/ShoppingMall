using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ShoppingMall.Models;
using Braintree;
using System.Data;


namespace ShoppingMall.Controllers
{
    public class HomeController : Controller
    {
        private ShoppingMall.Models.ShoppingCartDatabaseContexts db = new ShoppingMall.Models.ShoppingCartDatabaseContexts();

        //
        // GET: /Home/

        public string RenderViewToString(string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            //ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string getProductBySlider(int startPrice, int endPrice, int pageNumber, Guid subcategoryid, Guid categoryid)
        {
            try
            {
                ViewBag.startPrice = startPrice;
                ViewBag.endPrice = endPrice;
                ViewBag.pageNumber = pageNumber;
                ViewBag.subcategoryid = subcategoryid;
                ViewBag.categoryid = categoryid;

                if (pageNumber != null && subcategoryid == Guid.Empty)
                {
                    string partialViewValue = "";
                    //var products = db.Products.Where(p => p.Price > startPrice && p.Price <= endPrice && p.Discount != null).ToList();// OrderBy(p => p.Price).Skip(6 * (pageNumber - 1)).Take(6).ToList();
                    partialViewValue = RenderViewToString("_SliderProducts");
                    return partialViewValue;
                }
                else if(pageNumber!= null && subcategoryid != null)
                {
                    string partialViewValue = "";
                    //var SubCategory = db.Products.Where(sc => sc.Price > startPrice && sc.Price <= endPrice && sc.SubcategoyID == subcategoryid).ToList();// .OrderBy(p => p.Price).Skip(6 * (pageNumber - 1)).Take(6).ToList();
                    partialViewValue = RenderViewToString("_SliderProducts");
                    return partialViewValue;
                }
                else if (pageNumber != null && categoryid != null)
                {
                    string partialViewValue = "";
                    //var SubCategory = db.Products.Where(sc => sc.Price > startPrice && sc.Price <= endPrice && sc.SubCategory.CategoryID == categoryid).ToList(); // OrderBy(p => p.Price).Skip(6 * (pageNumber - 1)).Take(6).ToList();
                    partialViewValue = RenderViewToString("_SliderProducts");
                    return partialViewValue;
                }
                
                
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult AccountSetting()
        {
            if (Session["UserFirstName"] == null)
                return RedirectToAction("Login", "Home");

            string Email = Session["UserName"].ToString();
            var user = db.Users.Where(u => u.EmailID == Email).ToList();

            return View(user[0]);
        }

        [HttpPost]
        public ActionResult AccountSetting(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(user).State = EntityState.Modified;
                    user.ModifiedDate = DateTime.Now;
                    db.SaveChanges();

                    TempData["ErrorMsg"] = "data has been updated successfully.";
                }
                else
                {
                    TempData["ErrorMsg"] = "Error: Something went wrong, please try again.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Error: " + ex.Message;
            }
            return View(user);
        }

        public ActionResult ProductDetails(Guid ProductID)
        {
            try
            {

                var product = db.Products.Where(p => p.ProductID == ProductID).Single();
                return View(product);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public string Login(string Email,string Password)
        {
            try
            {
                var user = db.Users.Where(u => u.EmailID == Email && u.Password == Password).ToList();

                if (user.Count > 0)
                {

                    Session["UserName"] = user[0].EmailID;
                    Session["UserFirstName"] = user[0].FristName + " " + user[0].LastName;
                    //return RedirectToAction("Index","Home");
                    return "Success";

                }
                else
                {
                    //return View();
                    return "Email or Password Incorrect, Please try again";
                }
            }
            catch(Exception ex)
            {
                return "Error: " + ex.Message;
            }
           
        }
        public ActionResult Logout()
        {
            Session["UserName"] = null;
            Session["UserFirstName"] = null;
            return RedirectToAction("Index","Home");
        }
        public ActionResult FeartureProduct()
        {
            try
            {
                var product = db.Products.Where(p => p.Discount != null).OrderByDescending(p => p.Discount).ToList();

                return View(product);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult AllFeartureProduct(int pageNum, Guid categoryid)
        {
            ViewBag.PageNumber = pageNum;
                return View();
         }
        public ActionResult AllLatestProduct(int pageNum, Guid categoryid)
        {
            ViewBag.PageNumber = pageNum;
            return View();
        }
        public ActionResult SearchBySubcategoryName(Guid subcategoryid, int pageNum, Guid categoryid)
        {
                 ViewBag.PageNumber = pageNum;
                 ViewBag.SubCategoryId = subcategoryid;
                 var SubCategory = db.Products.Where(sc => sc.SubcategoyID == subcategoryid).OrderBy(sc => sc.Price).ToList();
                return View(SubCategory);
           
            
        }

        [HttpPost]
        public JsonResult ProductNameList(string ProductName)
        {
            try
            {
                var productNameList = (from p in db.Products
                                       where p.ProductName.Contains(ProductName)
                                       select new { p.ProductName, p.ProductID }).Take(10).ToList();

                return Json(productNameList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { ProductName = "", ProductID = "" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SearchByCategoryAndProductName(int pageNum)
        {
            
            ViewBag.PageNumber = pageNum;
            ViewBag.SubCategoryId = Guid.Empty;
            var Products = db.Products.ToList();
            return View(Products);
        }


        public ActionResult SearchByCategoryAndProductName1(Guid CategoryId, string ProductName, int pageNum, Guid SubCategoryId)
        {
            ViewBag.SubCategoryId = Guid.Empty;
            ViewBag.PageNumber = pageNum;
            if (CategoryId == Guid.Empty && ProductName == "")
            {
               

                ViewBag.category = Guid.Empty;
                var Products = db.Products.ToList();
                return View("SearchByCategoryAndProductName",Products);
            }
            else if (CategoryId == Guid.Empty && ProductName != "")
            {
                var Products = db.Products.Where(sc => sc.ProductName.Contains(ProductName)).OrderBy(sc=>sc.Price).ToList();
                return View("SearchByCategoryAndProductName",Products);
            }
            else if (CategoryId != Guid.Empty && (ProductName == null || ProductName == ""))
            {
                ViewBag.category = CategoryId;
                var products = (from p in db.Products
                                join sc in db.SubCategories
                                on p.SubcategoyID equals sc.SubCategoryID
                                where sc.CategoryID == CategoryId
                                select p).OrderBy(sc => sc.Price).ToList();
                return View("SearchByCategoryAndProductName",products);
            }

            else
            {
               var products = (from p in db.Products
                                join sc in db.SubCategories
                                on p.SubcategoyID equals sc.SubCategoryID
                                where sc.CategoryID == CategoryId && p.ProductName.Contains(ProductName)
                               select p).OrderBy(sc => sc.Price).ToList();

                return View("SearchByCategoryAndProductName",products);
            }
            
        }

        //  if(Guid.Empty == categoryId && ProductName == "")
        // else if(Guid.Empty == categoryId && ProductName != "")
        //  else if(Guid.Empty != categoryId && ProductName == "")
        //  else both having values categoryid and productname
        //sc.ProductName.Contains(ProductName)
         
        public ActionResult CartProduct()
        {
            if (Session["UserName"] == null)
                return RedirectToAction("LoginNow");

            return View();
        }

        public ActionResult CheckLogin()
        {
            if (Session["UserName"] == null)
                return RedirectToAction("LoginNow");
            else
                return RedirectToAction("PayNow");
        }

        public ActionResult LoginNow()
        {
            return View();
           
        }

        [HttpPost]
        public ActionResult LoginNow(string Email, string Password)
        {
            try
            {
                var user = db.Users.Where(u => u.EmailID == Email && u.Password == Password).ToList();

                if (user.Count > 0)
                {

                    Session["UserName"] = user[0].EmailID;
                    Session["UserFirstName"] = user[0].FristName + " " + user[0].LastName;
                    return RedirectToAction("PayNow","Home");

                }
                else
                {
                    ViewBag.ErrorMsg = "Email or Password Incorrect, Please try again";
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult Registration()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Registration(User user)
        {
            user.UsersID = Guid.NewGuid();
            user.ModifiedDate = DateTime.Now;
            user.CreateDate = DateTime.Now;
            user.IsAdmin = false;

            db.Users.Add(user);
            db.SaveChanges();

            Session["UserName"] = user.EmailID;
            Session["FirstName"] = user.FristName;

            return RedirectToAction("PayNow","Home");

        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            try
            {
                var user = db.Users.Where(u => u.EmailID == Email).ToList();
                
                if (user.Count > 0)
                {
                    GeneralUtility generalUtility = new GeneralUtility();

                    string mailBody = System.IO.File.ReadAllText(Server.MapPath("~/MailTemplate/ForgotPassword.html"));
                    mailBody = mailBody.Replace("#FirstName", user[0].FristName);
                    mailBody = mailBody.Replace("#EmailId", user[0].EmailID);
                    mailBody = mailBody.Replace("#Password", user[0].Password);

                    generalUtility.SendEmail(Email, "Password Recovery!", mailBody);
                    ViewBag.ErrorMsg = "Password has been sent to you, please check it.";
                }
                else
                {
                    ViewBag.ErrorMsg = "We didn't recognize email id you provided, please try again.";
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AddtoCart(Guid ProductId)
        {
            try
            {
                List<AddtoCart> products = new List<AddtoCart>();
                var product = db.Products.Where(p => p.ProductID == ProductId).Single();
                AddtoCart addtoCart = new Models.AddtoCart();

                addtoCart.ProductId = product.ProductID;
                addtoCart.Discount = product.Discount;
                addtoCart.Price = product.Price;
                addtoCart.ProductDescription = product.ProductDescription;
                addtoCart.ProductName = product.ProductName;
                addtoCart.ProductImage = product.ProductImage;
                addtoCart.Size = product.Size;
                addtoCart.Qty = 1;
                addtoCart.MaxProduct = product.Qty;
                

                if (Session["AddtoCartItems"] != null)
                {
                    var list = Session["AddtoCartItems"] as List<AddtoCart>;
                    products = list;
                }

                //AddtoCart a = products.Find(p => p.ProductId == ProductId);
                //if (a != null && a.ProductName != "")
                //{

                //}
                int isMatched = 0;
                for (int i = 0; i < products.Count; i++)
                {
                    if (products[i].ProductId == ProductId)
                    {
                        products[i].Qty = products[i].Qty + 1;
                        isMatched = 1;
                        break;
                    }
                }

                if (isMatched == 0)
                {
                    products.Add(addtoCart);
                }

                Session["AddtoCartItems"] = products;

                return RedirectToAction("ViewCart");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ViewCart()
        {
            if (Session["AddtoCartItems"] == null)
                return RedirectToAction("Index");

            List<AddtoCart> addtoCart = Session["AddtoCartItems"] as List<AddtoCart>;
            return View(addtoCart);
        }

        [HttpPost]
        public string ChangePriceByQty(int Qty, Guid ProductId)
        {
            try
            {
                List<AddtoCart> products = new List<AddtoCart>();
                //var product = db.Products.Where(p => p.ProductsId == ProductId).Single();

                if (Session["AddtoCartItems"] != null)
                {
                    var list = Session["AddtoCartItems"] as List<AddtoCart>;
                    products = list;
                }

                for (int i = 0; i < products.Count; i++)
                {
                    if (products[i].ProductId == ProductId)
                    {
                        products[i].Qty = Qty;
                        break;
                    }
                }

                Session["AddtoCartItems"] = products;

                return "Success";
            }
            catch (Exception)
            {
                return "Unsuccess";
            }
        }

        [HttpPost]
        public string RemoveAddtoCartProduct(int index)
        {
            try
            {
                List<AddtoCart> addtoCart = Session["AddtoCartItems"] as List<AddtoCart>;
                addtoCart.RemoveAt(index);

                if (addtoCart.Count == 0)
                {
                    Session["AddtoCartItems"] = null;
                    Session["GrandTotal"] = null;
                }
                else
                {

                    Session["AddtoCartItems"] = addtoCart;
                    
                }

                return "Success";
            }
            catch (Exception ex)
            {
                return "Unsuccess";
            }
        }

        public ActionResult PayNow()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PayNow(PayNow payNow)
        {

            try
            {
                string transactionId = "";
                BraintreeGateway gateway = new BraintreeGateway(Braintree.Environment.SANDBOX, "yfdwsyrvnk3szzvx", "n6n49xttqyjyfqtt", "e3bdf8c24b739625011a335a2b382ab3");
                payNow.Amount = Convert.ToDecimal(Session["GrandTotal"]);
                var request = new TransactionRequest
                {
                    Amount = payNow.Amount,
                    CreditCard = new TransactionCreditCardRequest
                    {
                        Number = payNow.CardNumber,
                        ExpirationDate = payNow.ExpMonth + "/" + payNow.ExpYear,
                        CVV = payNow.CVV
                    }
                };


                Result<Transaction> result = gateway.Transaction.Sale(request);
                transactionId = result.Target.Id;

                var list = Session["AddtoCartItems"] as List<AddtoCart>;


                string Email = Session["UserName"].ToString();
                Guid userId = db.Users.Where(u => u.EmailID == Email).Single().UsersID;

                //  generate Order
                Order order = new Order();
                decimal? TaxAmount = Convert.ToDecimal(Session["TaxAmount"].ToString());
                decimal? TotalAmount = Convert.ToDecimal(Session["GrandTotal"].ToString());

                order.OrderID = Guid.NewGuid();
                order.UsersID = userId;
                order.OrderDate = DateTime.Now;
                order.TaxAmount = TaxAmount;
                order.TotalAmount = TotalAmount;
                order.CreateDate = DateTime.Now;
                order.ModifiedDate = DateTime.Now;

                db.Orders.Add(order);
                db.SaveChanges();

                //  generate Order Details
                foreach (var product in list)
                {
                    if (product != null)
                    {
                        OrderDetail orderDetail = new OrderDetail();

                        orderDetail.OrderDetailID = Guid.NewGuid();
                        orderDetail.OrderID = order.OrderID;
                        orderDetail.ProductID = product.ProductId;
                        orderDetail.Qty = product.Qty;
                        
                        orderDetail.Price = product.Price;
                        orderDetail.Size = product.Size;
                        orderDetail.CreateDate = DateTime.Now;
                        orderDetail.ModifiedDate = DateTime.Now;

                        var product1 = db.Products.Find(product.ProductId);

                        if(product1 != null)
                        {
                            product1.Qty = (product1.Qty - product.Qty);
                            db.Entry(product1).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        db.OrderDetails.Add(orderDetail);
                        db.SaveChanges();


                        //  update query ProductStock

                    }
                }

                //  generate Payment Info

                PaymentInfo paymentInfo = new PaymentInfo();
                paymentInfo.PaymentInfoID = Guid.NewGuid();
                paymentInfo.TransationID = transactionId;
                paymentInfo.OrderID = order.OrderID;
                paymentInfo.CreateDate = DateTime.Now;
                paymentInfo.ModifiedDate = DateTime.Now;

                db.PaymentInfoes.Add(paymentInfo);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ShippingAddress");

            //return RedirectToAction("Success");
        }

        public ActionResult ShippingAddress()
        {
            try
            {
                if (Session["UserName"] == null)
                    return RedirectToAction("Index");

                string Email = Session["UserName"].ToString();
                var user = db.Users.Where(u => u.EmailID == Email).Single();

                Shipping shipping = new Shipping();
                shipping.Address = user.Address;
                shipping.City = user.City;
                shipping.Country = user.Country;
                shipping.PhoneNumber = user.PhoneNumber;
                shipping.State = user.State;
                shipping.ZipCode = user.ZipCode;

                return View(shipping);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult ShippingAddress(Shipping shippingModel)
        {

            try
            {
                if (Session["UserName"] == null)
                    return RedirectToAction("Index");

                string Email = Session["UserName"].ToString();

                Guid userId = db.Users.Where(u => u.EmailID == Email).Single().UsersID;
                Guid orderId = db.Orders.Where(o => o.UsersID == userId).OrderByDescending(o => o.CreateDate).First().OrderID;

                //  generate shipping address

                Shipping shipping = new Shipping();

                shipping.ShippingID = Guid.NewGuid();
                shipping.OrderID = orderId;
                shipping.Address = shippingModel.Address;
                shipping.City = shippingModel.City;
                shipping.State = shippingModel.State;
                shipping.Country = shippingModel.Country;
                shipping.PhoneNumber = shippingModel.PhoneNumber;
                shipping.ZipCode = shippingModel.ZipCode;
                shipping.CreateDate = DateTime.Now;
                shipping.ModifiedDate = DateTime.Now;

                db.Shippings.Add(shipping);
                db.SaveChanges();

                //  send email of orderered products

                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Success()
        {
            try
            {
                Session["GrandTotal"] = null;
                Session["TaxAmount"] = null;
                Session["AddtoCartItems"] = null;

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult FilterByPrice()
        {
            return View();
        }
    }
}
