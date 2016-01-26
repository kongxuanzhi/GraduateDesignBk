using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GraduateDesignBk.Models
{

    public class ListRoleModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public string WhoCreate { get; set; }
        public string Name { get; set; }
        public int  RoleMemCount { get; set; }
    }

    public class AddNewRoleModel
    {
        [Required]
        [Display(Name ="角 色 名：")]
        public string RoleName { get; set; }

        [Display(Name = "角色 描述：")]
        public string Description { get; set; }
    }
    public class ChangeRoleModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "角 色 名：")]
        public string RoleName { get; set; }
        [Display(Name = "角色 描述：")]
        public string Description { get; set; }
    }
}