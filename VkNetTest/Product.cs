using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkNetTest
{
    class Product
    {
        // Unique product ID which given by method SetupUID;
        public int PID;
        public static List<Product> Products { get; private set; } = new List<Product>();


        /// TODO: impletent AddProduct
        public static void AddProduct(Product prod)
            => throw new NotImplementedException();

        // TODO: setup unique ID
        public void SetupUID()
            => throw new NotImplementedException();
    }
}
