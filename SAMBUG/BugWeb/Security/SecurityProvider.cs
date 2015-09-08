using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugBusiness.Interface.BugSecurity.DTO;

namespace BugWeb.Security
{
    public class SecurityProvider
    {
        //define role types for all roles
        private const int GROWER_ROLE = 1;
        private const int ADMIN_ROLE = 2;

        private static bool isLoggedIn(HttpSessionStateBase session)
        {
            return (session.Count != 0);
        }

        public static bool isAdmin(HttpSessionStateBase session)
        {
            if (!isLoggedIn(session))
            {
                return false;
            } else
            {
                UserDTO user = (UserDTO)session["UserInfo"];
                if (user.Roles.SingleOrDefault(role => role.RoleType.Equals(ADMIN_ROLE))==default(RoleDTO))
                {
                    return false;
                }
                return true;
            }
        }

        public static bool isGrower(HttpSessionStateBase session)
        {
            if (!isLoggedIn(session))
            {
                return false;
            }
            else
            {
                UserDTO user = (UserDTO)session["UserInfo"];
                if (user.Roles.SingleOrDefault(role => role.RoleType.Equals(GROWER_ROLE)) == null)
                {
                    return false;
                }
                return true;
            }
        }
    }
}