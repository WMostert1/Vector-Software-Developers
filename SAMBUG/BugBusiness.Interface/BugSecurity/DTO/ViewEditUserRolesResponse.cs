using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface.Domain;

//Used to list the users in order to edit their roles by an administrator
namespace BugBusiness.Interface.BugSecurity.DTO
{
    public class ViewEditUserRolesResponse
    {
        public ICollection<User> Users { get; set; }
    }
}
