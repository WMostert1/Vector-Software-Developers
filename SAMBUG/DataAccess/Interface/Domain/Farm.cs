using System.Collections.Generic;

namespace DataAccess.Interface.Domain
{
    public class Farm
    {
        public long FarmID { get; set; }
        public string FarmName { get; set; }
        public List<Block> Blocks { get; set; }
    }
}
