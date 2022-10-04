using System;
namespace JwtIdentityLib.Jwt
{
    /// <summary>
    /// User model.
    /// </summary>
    public class UserModel
    {
        public Guid ID { get; set; }

        public string? UserName { get; set; }
    }
}

