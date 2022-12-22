using System;
using System.Security.Cryptography;
using System.Text;

namespace Mephi.Cyber.AuthService.Core.Utils
{
    public class Password
    {
        private string _password;
        private int _salt;

        public Password(string strPassword, int nSalt)
        {
            _password = strPassword;
            _salt = nSalt;
        }
        public static int CreateRandomSalt()
        {
            Byte[] _saltBytes = new Byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(_saltBytes);

            return (_saltBytes[0] << 24) + (_saltBytes[1] << 16) +
              (_saltBytes[2] << 8) + _saltBytes[3];
        }
        public string ComputeSaltedHash()
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            Byte[] _secretBytes = encoder.GetBytes(_password);

            Byte[] _saltBytes = new Byte[4];
            _saltBytes[0] = (byte)(_salt >> 24);
            _saltBytes[1] = (byte)(_salt >> 16);
            _saltBytes[2] = (byte)(_salt >> 8);
            _saltBytes[3] = (byte)(_salt);

           
            Byte[] toHash = new Byte[_secretBytes.Length + _saltBytes.Length];
            Array.Copy(_secretBytes, 0, toHash, 0, _secretBytes.Length);
            Array.Copy(_saltBytes, 0, toHash, _secretBytes.Length, _saltBytes.Length);

            SHA1 sha1 = SHA1.Create();
            Byte[] computedHash = sha1.ComputeHash(toHash);

            return encoder.GetString(computedHash);
        }
    }
    public class RandomPasswordGenerator
    {

        const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
        const string UPPER_CAES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string NUMBERS = "123456789";
        const string SPECIALS = @"!@£$%^&*()#€";


        public static string GeneratePassword(bool useLowercase=true, bool useUppercase=true, bool useNumbers=true, bool useSpecial=true,
            int passwordSize=10)
        {
            char[] _password = new char[passwordSize];
            string charSet = "";
            System.Random _random = new Random();
            int counter;

            
            if (useLowercase) charSet += LOWER_CASE;

            if (useUppercase) charSet += UPPER_CAES;

            if (useNumbers) charSet += NUMBERS;

            if (useSpecial) charSet += SPECIALS;

            for (counter = 0; counter < passwordSize; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);
        }
    }

}

