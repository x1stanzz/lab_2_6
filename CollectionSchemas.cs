using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_6
{
    internal class CollectionSchemas
    {
        public static readonly Dictionary<string, List<string>> Schemas = new Dictionary<string, List<string>>
        {
            { "authors", new List<string> { "Name" } },
            { "books", new List<string> { "Name", "AuthorId", "PublisherId", "GenreId", "Isbn", "Description" } },
            { "customer", new List<string> { "Name", "Email", "Address.Country", "Address.City", "Address.Street", "Address.PostalCode"} },
            { "genres", new List<string> { "Name", "Description" } },
            { "publishers", new List<string> {"Name", "Country"} },
            { "reviews",new List<string> {"BookId", "CustomerId", "Review", "Date"}  }
        };
    }
}
