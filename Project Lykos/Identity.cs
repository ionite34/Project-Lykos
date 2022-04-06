using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos
{
    public class Identity
    {
        // Holds Identity such as Name and UUID for ProcessControl objects
        public int? UI_TotalCount { get; set; }
        public string? UI_Name { get; set; }
        public string UUID { get; }
        // Constructor
        public Identity()
        {
            // Generate UUID
            UUID = Guid.NewGuid().ToString();
        }
    }
}
