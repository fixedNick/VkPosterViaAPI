using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkNetTest
{
    class Product
    {
        // FileLogger.
        private static readonly ILogger Logger = new FileLogger();
        // Variable for setup new product id; Loading at start of program from file config.txt;
        private static int lastUserPID = -1;
        public static int LastUsedPID
        {
            get => lastUserPID;
            set
            {
                if (lastUserPID >= 0)
                    throw new Exception("Last ID already exists");
                lastUserPID = value;
            }
        }

        // Unique product ID which given by method SetupUID;
        public int PID { get; private set; }
        // Product name
        public string Name { get; protected set; }
        // All about product in text
        public string Description { get; protected set; }
        // Product price
        public double Price { get; protected set; }
        // List of local files path to photos of product
        public List<string> Photos { get; protected set; } = new List<string>();
        // Collection of all products
        public static List<Product> Products { get; private set; } = new List<Product>();

        /// <summary>
        /// Main costructor.
        /// </summary>
        /// <param name="addToCollection">TRUE - Add new product and give PID to it. FALSE - Creating object w/o adding to products list</param>
        public Product(string name, string desc, double price, List<string> photos = null, bool addToCollection = false)
        {
            Name = name;
            Description = desc;
            Price = price;
            if (photos?.Count > 0)
                Photos = photos;

            if (addToCollection == true)
                AddProduct(this);
        }

        /// <summary>
        /// Add new product into pool of all products.
        /// </summary>
        /// <param name="prod">Object of Product type which we gonna save</param>
        protected static void AddProduct(Product prod)
        {
            if (SetupPID(prod) == false)
                throw new Exception("Не удалось назначить ID для продукта");

            Products.Add(prod);
            Logger.Print($"Product | Товар '{prod.Name}' успешно добавлен в общий пул товаров.");
        }

        /// <summary>
        /// Setup Unique ID to product.
        /// </summary>
        /// <param name="prod">Object of Product</param>
        /// <returns>
        /// TRUE - PID Successfully setuped
        /// FALSE - An exception thrown
        /// </returns>
        protected static bool SetupPID(Product prod)
        {
            try
            {
                prod.PID = ++LastUsedPID;
                Logger.Print($"Product | Товару {prod.Name} назначен PID: {LastUsedPID}");
            }
            catch
            {
                Logger.Print($"Product | Не удалось назначить PID товару {prod.Name}");
                return false;
            }
            return true;
        }
    }
}
