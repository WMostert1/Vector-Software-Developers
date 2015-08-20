using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface.Domain
{
    public class Farm
    {
        public long FarmID { get; set; }
        public string FarmName { get; set; }
        public List<Block> Blocks { get; set; }
    }
}
