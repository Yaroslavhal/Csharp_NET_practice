using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinnesLogicLayer.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Patronymic { get; set; }
        [Required]
        public int? Age { get; set; }
        [DataType(DataType.Currency)]
        [Required]
        public float? Salary { get; set; }
    }
}
