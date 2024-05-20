using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "mongodb://localhost:27017";
            var database = "book_store_db";
            var mongoService = new DatabaseManagement(connectionString, database);

            while(true)
            {
                Console.WriteLine("Select an operation: ");
                Console.WriteLine("1. InsertDocument");
                Console.WriteLine("2. View documents in a collection");
                Console.WriteLine("3. Update document");
                Console.WriteLine("4. Delete document");
                Console.WriteLine("5. Exit");

                Console.Write("Option: ");
                int option = int.Parse(Console.ReadLine());

                switch(option)
                {
                    case 1:
                        string selectedCollection1 = mongoService.ChooseCollection();
                        BsonDocument newDocument1 = mongoService.CreateDocumentDynamically(selectedCollection1);
                        mongoService.InsertDocument(selectedCollection1, newDocument1);

                        Console.WriteLine("Document inserted successfullly");
                        break;
                    case 2:
                        string selectedCollection2 = mongoService.ChooseCollection();
                        mongoService.ViewDocuments(selectedCollection2);
                        break;
                    case 3: 
                        string selectedCollection3 = mongoService.ChooseCollection(); 
                        mongoService.UpdateDocument(selectedCollection3);
                        break;
                    case 4:
                        string selectedCollection4 = mongoService.ChooseCollection();
                        mongoService.DeleteDocument(selectedCollection4);
                        break;
                    case 5:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }

            
        }
    }
}
