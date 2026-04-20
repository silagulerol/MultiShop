using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ContactDtos;
using MultiShop.WebUI.Services.CatalogServices.ContactService;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public IActionResult Index()
        {
            ViewBag.directory1 = "Home";
            ViewBag.directory2 = "Contact";
            ViewBag.directory3 = "Send Message";
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Index(CreateContactDto createContactDto)
        {
            //formdaki eksik kısımlar
            createContactDto.SendDate = DateTime.Now;
            createContactDto.IsRead = false;

            await _contactService.AddContactAsync(createContactDto);
            return RedirectToAction("Index", "Default");
        }
    }
}
