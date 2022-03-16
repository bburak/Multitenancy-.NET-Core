using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product : BaseEntity, IMustHaveTenant
    {
        // This constructor was created for compelling certain create object use.
        public Product(string name, string description, int rate)
        {
            Name = name;
            Description = description;
            Rate = rate;
        }
        protected Product()
        {

        }
        public string Name { get; private set; }
        public string Description { get; private set; }
        protected int Rate { get; private set; }
        public string TenantId { get; set; }
    }
}
