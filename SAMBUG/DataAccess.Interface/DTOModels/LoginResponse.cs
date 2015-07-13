using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface.DTOModels
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public List<string> Role { get; set; }
    }
}
