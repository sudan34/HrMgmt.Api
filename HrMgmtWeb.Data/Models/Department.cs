using System.ComponentModel.DataAnnotations;

namespace HrMgmtWeb.api.Models
{
    public class Department
    {
        [Key]
        public int DeptId { get; set; }
        public string DeptName { get; set; }
    }
}
