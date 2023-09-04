using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartApi.Domain.Entities
{
    public class ApiUser : IdentityUser<long>
    {
        
    }

    public class Role : IdentityRole<long> { }
}
