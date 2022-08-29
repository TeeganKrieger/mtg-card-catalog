using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace MTGCC.Database
{
    public class AppUser
    {
        [Key]
        [Column(TypeName = "nvarchar(36)")]
        public string ID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string SessionToken { get; set; }

        public AppUser()
        {
            this.ID = null;
            this.Username = null;
            this.PasswordHash = null;
            this.PasswordSalt = null;
        }

        public AppUser(string username, string password)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username), "Email cannot be null!");
            if (username.Length > 50)
                throw new ArgumentException(nameof(username), "Email must be less than 50 characters!");

            HMACSHA512 hmac = new HMACSHA512();
            Guid guid = Guid.NewGuid();

            this.ID = guid.ToString();
            this.Username = username;
            this.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            this.PasswordSalt = hmac.Key;
            this.SessionToken = "";
        }

        public bool CheckPassword(string checkAgainst)
        {
            HMACSHA512 hmac = new HMACSHA512(PasswordSalt);

            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(checkAgainst));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != PasswordHash[i]) return false;
            }
            return true;
        }
    }
}
