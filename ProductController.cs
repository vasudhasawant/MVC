using _3_MVCCRUDUSINGCRUD.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;


namespace _3_MVCCRUDUSINGCRUD.Controllers
{
    public class ProductController : Controller
    {
        ProductDBContext _db = new ProductDBContext();
        // GET: Product
        [HttpGet]
        public ActionResult Index(string search,string sort,int? page,int? recordsPerPage)
        {

            List<product> productss = _db.products.ToList();
            List<Category> categorys = _db.Categories.ToList();

            ViewBag.NameSort = (sort == "name") ? "name desc" : "name";
            ViewBag.PriceSort = (sort == "price") ? "price desc" : "price";
            ViewBag.CnameSort = (sort == "cname") ? "cname desc" : "cname";
            if (!string.IsNullOrEmpty(search))
            {
                productss = _db.products
                    .Where(p=>p.Name.Contains(search)||
                    p.price.ToString().Equals(search)||
                    p.Category.Name.Contains(search)).ToList();
            }

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "name":
                        productss= productss.OrderBy(p=>p.Name).ToList();
                        break;
                    case "price":
                        productss = productss.OrderBy(p => p.price).ToList();
                        break;
                    case "price desc":
                        productss = productss.OrderByDescending(p => p.price).ToList();
                        break;
                    case "cname":
                        categorys = categorys.OrderBy(p => p.Name).ToList();
                        break;
                    case "cname desc":
                        categorys = categorys.OrderByDescending(p => p.Name).ToList();
                        break;
                    default:
                        productss = productss.OrderByDescending(p => p.Name).ToList();
                        break;
                } 
            }
          
            return View(productss.ToPagedList(page??1, recordsPerPage?? 8));
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Categories = _db.Categories.ToList().
                Select(c => new SelectListItem() { Text = c.Name, Value = c.CategoryId.ToString() });



            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind(Exclude ="CreatedDate")] product products)
        {
            try
            {
                products.CreateDate = DateTime.Now;
                if(products.Image != null && products.Image.ContentLength>0) 
                {
                    string fileName = products.Image.FileName;

                    string filderPath = Server.MapPath("~/Images");
                    string filepath=Path.Combine(filderPath, fileName);

                    products.Image.SaveAs(filepath);

                    products.ImagePath = $"/Images/{fileName}";

                }      
                _db.products.Add(products);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(products);
            }
        }



        [HttpGet]
        [ActionName("Delete")]
        public ActionResult Delete(int? id)
        {
            var product = _db.products.Find(id);
            return View(product);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConf(int? Id)
        {
            var product = _db.products.Find(Id);
            _db.products.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Details(int? id)
        {
            product products = _db.products.Find(id);
            return View(products);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
           

            product products = _db.products.Find(id);
            ViewBag.Categories = _db.Categories.ToList().
           Select(c => new SelectListItem() { Text = c.Name, Value = c.CategoryId.ToString() });

            return View(products);
        }
        [HttpPost]
        public ActionResult Edit(product products)
        {
            try
            {
                product dbproduct = _db.products.Find(products.Id);

                dbproduct.Name = products.Name;
                dbproduct.price = products.price;
                dbproduct.CategoryId = products.CategoryId;
                if (products.Image != null && products.Image.ContentLength > 0)
                {
                    string fileName = products.Image.FileName;

                    string filderPath = Server.MapPath("~/Images");
                    string filepath = Path.Combine(filderPath, fileName);

                    products.Image.SaveAs(filepath);

                    dbproduct.ImagePath = $"/Images/{fileName}";

                }
               
              

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch{
                return View(products);
            }

           
        }
        [HttpPost]
        public ActionResult Deletemultiple(IEnumerable<int> productToDelete)
        {
            foreach (var id in productToDelete)
            {
                 var product=_db.products.Find(id);
                _db.products.Remove(product);
                _db.SaveChanges();
            }


            return RedirectToAction("Index");
        }
    }
}