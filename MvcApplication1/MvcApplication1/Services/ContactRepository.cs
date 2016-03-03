using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Services
{
    public class ContactRepository
    {
        private const string CacheKey = "ContactStore";

        public ContactRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var contacts = new Contact[]
                    {
                        new Contact
                        {
                            Id = 1, Name = "Glenn Block"
                        },
                        new Contact
                        {
                            Id = 2, Name = "Dan Roth"
                        }
                    };

                    ctx.Cache[CacheKey] = contacts;
                }
            }
        }

        private void UpdateContactStore(Contact[] updatedList)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                ctx.Cache[CacheKey] = updatedList;
            }
        }

        public Contact[] GetAllContacts()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                return (Contact[])ctx.Cache[CacheKey];
            }

            return new Contact[]
            {
                new Contact
                {
                    Id = 0,
                    Name = "Placeholder"
                }
            };
        }

        public Contact GetContact(int id)
        {
            return this.GetAllContacts().FirstOrDefault(t => t.Id == id);
        }

        public bool UpdateContact (Contact contact)
        {
            var repContact = GetContact(contact.Id);
            if (repContact != null)
            {
                var contacts = GetAllContacts().ToList();
                var index = contacts.IndexOf(repContact);
                if (index != -1)
                {
                    contacts.RemoveAt(index);
                    contacts.Insert(index, contact);
                    UpdateContactStore(contacts.ToArray());
                    return true;
                }
            }
            return false;
        }

        public bool SaveContact(Contact contact)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Contact[])ctx.Cache[CacheKey]).ToList();
                    currentData.Add(contact);
                    ctx.Cache[CacheKey] = currentData.ToArray();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }

            return false;
        }
    }
}