using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using BugBusiness.Interface.BugSecurity.DTO;

namespace BugWeb.Security
{
    public class AuthenticateAttribute : FilterAttribute, IAuthenticationFilter
    {
        public string Roles { get; set; }
        public bool Alternate { get; set; }


        public void OnAuthentication(AuthenticationContext context)
        {
            if (context.HttpContext.Session != null && 
                context.HttpContext.Session.Count != 0) //if authenticated
            {
                //obtain signed in user
                UserDTO user = (UserDTO) context.HttpContext.Session["UserInfo"]; 

                //authorise the user
                if (Alternate) //if any of the roles suffice
                {
                   bool atLeastOneRole = false;
                   
                    foreach (var role in Roles.Split(','))
                    {
                        if (user.Roles.Select(rle => rle).Where(rle => rle.RoleType == long.Parse(role)).ToList().Any())
                        {
                            atLeastOneRole = true;
                            break;
                        }
                    } 

                    if (!atLeastOneRole)
                        context.Result = new HttpUnauthorizedResult(); //mark unauthorised
                }
                else
                {
                    foreach (var role in Roles.Split(','))
                    {
                        //if one of the required roles is not found in the user's roles, the user is not
                        //authorised
                        if (!user.Roles.Select(rle => rle).Where(rle => rle.RoleType == long.Parse(role)).ToList().Any())
                            context.Result = new HttpUnauthorizedResult(); //mark unauthorised
                    }
                }
            }
            else
            {
                context.Result = new HttpUnauthorizedResult(); // mark unauthorised
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            if (context.Result == null || context.Result is HttpUnauthorizedResult)
            {
                context.Result = new RedirectToRouteResult("Default",
                    new System.Web.Routing.RouteValueDictionary{
                        {"controller", "home"},
                        {"action", "index"}
                    });
            }
        }
    }
}