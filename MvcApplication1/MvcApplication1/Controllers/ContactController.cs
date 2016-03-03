using MvcApplication1.Attributes;
using MvcApplication1.Models;
using MvcApplication1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvcApplication1.Controllers
{
    [RequireHttps]
    public class ContactController : ApiController
    {
        private ContactRepository contactRepository;

        public ContactController()
        {
            this.contactRepository = new ContactRepository();
        }

        [RequireHttps]
        public Contact[] Get()
        {
            return contactRepository.GetAllContacts();
        }

        [RequireHttps]
        public IHttpActionResult GetContact(int id)
        {
            var contact = contactRepository.GetContact(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [RequireHttps]
        [Authorize]
        public HttpResponseMessage Post(Contact contact)
        {
            this.contactRepository.SaveContact(contact);
            var response = Request.CreateResponse<Contact>(System.Net.HttpStatusCode.Created, contact);
            return response;
        }

        [RequireHttps]
        [Authorize]
        public IHttpActionResult Put(Contact contact)
        {
            if (this.contactRepository.UpdateContact(contact))
            {
                return Ok(contact);
            }
            return NotFound();
        }
    }
}
