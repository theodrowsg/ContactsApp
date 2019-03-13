namespace ContactsApp.API.Helpers
{
    public static class SecurityService
    {
        public static bool ValidatePasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
               var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
               for(int i = 0; i< passwordHash.Length; i++){
                  if(computedHash[i]!= passwordHash[i]) return false;
               }
            }

            return true;
            
        }

        public static  void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}