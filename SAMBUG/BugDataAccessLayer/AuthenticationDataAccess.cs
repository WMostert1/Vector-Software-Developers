using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugDataAccessLayer
{
    class AuthenticationDataAccess
    {
        private BugDBEntities _dbContext = new BugDBEntities();

        public bool Login(string username, string password)
        {
            return false;
        }
    }
}
