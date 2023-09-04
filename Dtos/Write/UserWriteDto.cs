using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.DTOS.WriteDtos
{
    public class UserWriteDto
    {
        public string UserName { get; set; }


        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
