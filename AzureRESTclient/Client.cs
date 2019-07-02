using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureRESTclient
{
    public class Client
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            int choice;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1 - Get all customers");
                Console.WriteLine("2 - Create new customer");
                Console.WriteLine("3 - Edit customer");
                Console.WriteLine("4 - Delete customer");
                Console.WriteLine("0 - Exit customer");
                Int32.TryParse(Console.ReadLine(), out choice);
                if (choice == 1)
                    client.Get();
                if (choice == 2)
                    client.Create(0);
                if (choice == 3)
                    client.Create(1);
                if (choice == 4)
                    client.Delete();
                if (choice == 0)
                    break;
            }
        }

        private void Get()
        {
            Console.Clear();
            var client = new HttpClient();
            var responseGetContentId = client.GetAsync("https://drozdovskiytest.azurewebsites.net/api/Customer/")
                .ConfigureAwait(false).GetAwaiter().GetResult();
            var content = responseGetContentId.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter()
                .GetResult();
            var result = JsonConvert.DeserializeObject<List<Customer>>(content);
            Console.WriteLine("|ID|FirstName|LastName|Email|PhoneNumber|Time of creation|");
            Console.WriteLine("==========================================================");
            foreach (var customer in result)
            {
                Console.Write('|'+customer.ID.ToString() + '|' + customer.FirstName + '|' + customer.LastName + '|' + customer.EmailAdress + '|' + customer.PhoneNumber + '|' + customer.RecordCreated + "|\n");
            }
            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }

        private void Create(int choice)
        {
            Console.Clear();
            var customer = new Customer();
            if (choice == 1)
            {
                Console.Write("ID=");
                customer.ID = Convert.ToInt32(Console.ReadLine());
            }
            customer.RecordCreated = DateTime.Now;
            Console.Write("Email=");
            customer.EmailAdress = Console.ReadLine();
            Console.Write("FirstName=");
            customer.FirstName = Console.ReadLine();
            Console.Write("LastName=");
            customer.LastName = Console.ReadLine();
            Console.Write("Phone=");
            customer.PhoneNumber = Console.ReadLine();
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(customer);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var responsePost = client.PostAsync("https://drozdovskiytest.azurewebsites.net/api/Customer/post", content)
                .ConfigureAwait(false).GetAwaiter().GetResult();
            var responseCode = responsePost.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter()
                .GetResult();
            if (responseCode == "")
            {
                Console.WriteLine("Incorrect input");
            }

            if (responseCode == "1")
            {
                Console.WriteLine("Successful execution");
            }

            if (responseCode == "2")
            {
                Console.WriteLine("Email is already taken");
            }

            if (responseCode == "3")
            {
                Console.WriteLine("Incorrect id");
            }
            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }

        private void Delete()
        {
            Console.Clear();
            Console.Write("ID=");
            int id= Convert.ToInt32(Console.ReadLine());
            var client = new HttpClient();
            var responseDelete = client.DeleteAsync("https://drozdovskiytest.azurewebsites.net/api/Customer/delete/" + id).ConfigureAwait(false).GetAwaiter().GetResult();
            var responseCode = responseDelete.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter()
                .GetResult();
            if (responseCode == "0")
            {
                Console.WriteLine("Incorrect id");
            }
            else
            {
                Console.WriteLine("Successful execution");
            }
            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
    }
}
