using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_6
{
    internal class DatabaseManagement
    {
        private readonly IMongoDatabase _database;

        public DatabaseManagement(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public List<string> GetCollections()
        {
            return _database.ListCollectionNames().ToList();
        }

        public string ChooseCollection()
        {
            var collections = GetCollections();
            Console.WriteLine("Available Collections: ");
            for (int i = 0; i < collections.Count; i++)
            {
                Console.WriteLine($"{i + 1}.{collections[i]}");
            }
            Console.Write("Select a collection (number): ");
            int choice = int.Parse( Console.ReadLine() );
            return collections[choice - 1];
        }
        
        public IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return _database.GetCollection<BsonDocument>(collectionName);
        }

        public void InsertDocument(string collectionName, BsonDocument document)
        {
            var collection = GetCollection(collectionName);
            collection.InsertOne(document);
        }

        public BsonDocument CreateDocumentDynamically(string collectionName)
        {
            var document = new BsonDocument();

            if(!CollectionSchemas.Schemas.ContainsKey(collectionName))
            {
                Console.WriteLine("Unknown collection schema. Please enter fields manually ('done' to finish): ");

                while(true)
                {
                    Console.Write("Field name: ");
                    string fieldName = Console.ReadLine();
                    if (fieldName.ToLower() == "done") break;

                    Console.Write("Field value: ");
                    string fieldValue = Console.ReadLine();

                    document.Add(fieldName, fieldValue);
                }
            } 
            else
            {
                var fields = CollectionSchemas.Schemas[collectionName];
                Console.WriteLine($"Enter values for the fields of the collection:");

                foreach(var field in fields)
                {
                    if (field.Contains("."))
                    {
                        var nestedFields = field.Split('.');
                        var nestedDoc = document;

                        for(int i = 0; i < nestedFields.Length - 1; i++)
                        {
                            if (!nestedDoc.Contains(nestedFields[i]))
                            {
                                nestedDoc.Add(new BsonElement(nestedFields[i], new BsonDocument()));
                            }
                            nestedDoc = nestedDoc[nestedFields[i]].AsBsonDocument;
                        }
                    }

                    Console.Write($"{field}: ");
                    string fieldValue = Console.ReadLine();
                    document.Add(field, fieldValue);
                }
            }
            return document;
        }

        public void ViewDocuments(string collectionName)
        {
            var collection = GetCollection(collectionName);
            var documents = collection.Find(new BsonDocument()).ToList();
            Console.WriteLine($"Documents in collection:");

            foreach(var document in documents)
            {
                Console.WriteLine("Document: ");
                foreach(var element in document.Elements)
                {
                    Console.WriteLine($"{element.Name}: {element.Value}");
                }
                Console.WriteLine();
            }
        }
        public void UpdateDocument(string collectionName)
        {
            var collection = GetCollection(collectionName);
            var documents = collection.Find(new BsonDocument()).ToList();

            Console.WriteLine($"Documents in {collectionName} collection:");

            foreach (var document in documents)
            {
                Console.WriteLine(document.ToJson());
            }

            Console.Write("Enter the _id of the document you want to update: ");
            string id = Console.ReadLine();

            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            var oldDocument = collection.Find(filter).FirstOrDefault();

            if (oldDocument == null)
            {
                Console.WriteLine("Document not found.");
                return;
            }

            Console.WriteLine("Enter new values for the fields:");

            var newDocument = CreateDocumentDynamically(collectionName);

            var updateBuilder = Builders<BsonDocument>.Update;
            var updateDefinitionList = new List<UpdateDefinition<BsonDocument>>();

            foreach (var element in newDocument.Elements)
            {
                var updateDefinition = updateBuilder.Set(element.Name, element.Value);
                updateDefinitionList.Add(updateDefinition);
            }

            var combinedUpdateDefinition = updateBuilder.Combine(updateDefinitionList);
            collection.UpdateOne(filter, combinedUpdateDefinition);

            Console.WriteLine("Document updated successfully.");
        }

        public void DeleteDocument(string collectionName)
        {
            var collection = GetCollection(collectionName);
            var documents = collection.Find(new BsonDocument()).ToList();
            Console.WriteLine($"Documents in collection:");
            foreach(var document in documents)
            {
                Console.WriteLine(document.ToJson());
            }

            Console.Write("Enter the _id of the document to delete: ");
            string id = Console.ReadLine();

            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            var result = collection.DeleteOne(filter);
            if(result.DeletedCount == 1)
            {
                Console.WriteLine("Document deleted successfully.");
            }
            else
            {
                Console.WriteLine("Document not found.");
            }
        }
    }
}
