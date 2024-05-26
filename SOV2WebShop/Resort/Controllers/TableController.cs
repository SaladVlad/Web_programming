using Resort.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Resort.Controllers
{
    public class TableController : Controller
    {
        // GET: Table
        public ActionResult Index()
        {
            List<Porudzbina> porudzbine = (List<Porudzbina>)(HttpContext.Application["porudzbine"]);

            return View(porudzbine);
        }

        [HttpPost]
        public ActionResult Add(int kod,string adresa,string brojTelefona,string proizvod,string nacinPlacanja,object checkbox)
        {
            Console.WriteLine("Type of checkbox: " + checkbox.GetType());
            List<Porudzbina> porudzbine = (List<Porudzbina>)(HttpContext.Application["porudzbine"]);

            if(porudzbine.Find(p=> p.Kod == kod) != null)
            {
                return View("AlreadyExists",kod);
            }
            else
            {
                porudzbine.Add(new Porudzbina(kod,adresa,brojTelefona,proizvod, nacinPlacanja));
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int kodZaPretragu)
        {
            List<Porudzbina> porudzbine = (List<Porudzbina>)(HttpContext.Application["porudzbine"]);

            if (porudzbine.Find(p => p.Kod == kodZaPretragu) == null)
            {
                return View("NonExistent",kodZaPretragu);
            }
            else
            {
                porudzbine.Remove(porudzbine.Find(p => p.Kod == kodZaPretragu));
                ViewBag.Message = "Uspesno obrisana porudzbina sa kodom:" + kodZaPretragu;
                return View("Index",porudzbine);
            }
        }
    }
}