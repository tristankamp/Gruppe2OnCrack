using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using SimpleServer.util;

namespace SimpleServer
{
    public class Slave
    {
        /// <summary>
        /// The algorithm used for encryption.
        /// Must be exactly the same algorithm that was used to encrypt the passwords in the password file
        /// </summary>
        private readonly HashAlgorithm _messageDigest;
        private readonly List<string> _dict;
        private UserInfo _userInfo;
        private UserInfoClearText _result;

        public Slave()
        {
            _messageDigest = new SHA1CryptoServiceProvider();
            _dict = new List<string>();
        }


        public void Work()
        {
            ConnectTcpWithAutoFlush(out TcpClient ballAndChain, out StreamWriter sw, out StreamReader sr);

            sw.WriteLine("0");
            string message = sr.ReadLine();
            while (message!= "1029384756")
            {
                _dict.Add(message);
                message = sr.ReadLine();
            }


            while (true)
            {
                sw.WriteLine("1");

                string password = sr.ReadLine();

                sw.WriteLine("Disconnect");
                ballAndChain.Close();
                

                if (password == "fuck af" || password is null)
                {
                    break;
                }

                string[] split = password.Split(':');
                _userInfo = new UserInfo(split[0], split[1]);

                RunCracking();

                var solvedPassword = !(_result is null) ? _result.Password : "Kunne ikke";
                _result = null;

                ConnectTcpWithAutoFlush(out ballAndChain, out sw, out sr);

                sw.WriteLine("2");
                sw.WriteLine(split[0] + " " + solvedPassword);
            }
        }

        private void ConnectTcpWithAutoFlush(out TcpClient client, out StreamWriter sw, out StreamReader sr)
        {
            client = new TcpClient();
            client.Connect(IPAddress.Loopback, 7);

            NetworkStream stream = client.GetStream();
            sr = new StreamReader(stream);
            sw = new StreamWriter(stream);

            sw.AutoFlush = true;
        }

        /// <summary>
        /// Runs the password cracking algorithm
        /// </summary>
        private void RunCracking()
        {
            int count = 0;
            int length = _dict.Count;
            while (_result is null && count < length)
            {
                CheckWordWithVariations(_dict[count++], _userInfo);
            }
        }

        /// <summary>
        /// Generates a lot of variations, encrypts each of the and compares it to an entry in the password file
        /// </summary>
        /// <param name="dictionaryEntry">A single word from the dictionary</param>
        /// <param name="userInfo">Username, encrypted password from the password file</param>
        private void CheckWordWithVariations(String dictionaryEntry, UserInfo userInfo)
        {
            String possiblePassword = dictionaryEntry;
            CheckSingleWord(userInfo, possiblePassword);

            String possiblePasswordUpperCase = dictionaryEntry.ToUpper();
            CheckSingleWord(userInfo, possiblePasswordUpperCase);

            String possiblePasswordCapitalized = StringUtilities.Capitalize(dictionaryEntry);
            CheckSingleWord(userInfo, possiblePasswordCapitalized);

            String possiblePasswordReverse = StringUtilities.Reverse(dictionaryEntry);
            CheckSingleWord(userInfo, possiblePasswordReverse);

            for (int i = 0; i < 100; i++)
            {
                String possiblePasswordEndDigit = dictionaryEntry + i;
                CheckSingleWord(userInfo, possiblePasswordEndDigit);
            }

            for (int i = 0; i < 100; i++)
            {
                String possiblePasswordStartDigit = i + dictionaryEntry;
                CheckSingleWord(userInfo, possiblePasswordStartDigit);
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    String possiblePasswordStartEndDigit = i + dictionaryEntry + j;
                    CheckSingleWord(userInfo, possiblePasswordStartEndDigit);
                }
            }
        }

        /// <summary>
        /// Checks a single word (or rather a variation of a word): Encrypts and compares to an entry in the password file
        /// </summary>
        /// <param name="userInfo">Username, encrypted password from the password file</param>
        /// <param name="possiblePassword">Username, encrypted password pair from the password file</param>
        private void CheckSingleWord(UserInfo userInfo, String possiblePassword)
        {
            char[] charArray = possiblePassword.ToCharArray();
            byte[] passwordAsBytes = Array.ConvertAll(charArray, PasswordFileHandler.GetConverter());

            byte[] encryptedPassword = _messageDigest.ComputeHash(passwordAsBytes);
            
            if (CompareBytes(userInfo.EntryptedPassword, encryptedPassword))  //compares byte arrays
            {
                _result = new UserInfoClearText(userInfo.Username, possiblePassword);
                //Console.WriteLine(userInfo.Username + " " + possiblePassword);
            }
        }

        /// <summary>
        /// Compares to byte arrays. Encrypted words are byte arrays
        /// </summary>
        /// <param name="firstArray"></param>
        /// <param name="secondArray"></param>
        /// <returns></returns>
        private static bool CompareBytes(IList<byte> firstArray, IList<byte> secondArray)
        {
            if (firstArray.Count != secondArray.Count)
            {
                return false;
            }
            for (int i = 0; i < firstArray.Count; i++)
            {
                if (firstArray[i] != secondArray[i])
                    return false;
            }
            return true;
        }

    }
}
