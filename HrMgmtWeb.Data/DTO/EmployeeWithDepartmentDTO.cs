using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrMgmtWeb.Data.DTO
{
    public class EmployeeWithDepartmentDTO
    {
        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int DeptId { get; set; }
        public string DeptName { get; set; }
    }
}
