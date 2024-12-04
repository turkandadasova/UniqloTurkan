using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity;

namespace UniqloMVC.Models
{
    public class User:IdentityUser
    {
        public string Fullname { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
