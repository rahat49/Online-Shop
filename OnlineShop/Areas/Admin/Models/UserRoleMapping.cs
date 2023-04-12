using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace OnlineShop.Areas.Admin.Models
{
    public class UserRoleMapping
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}
