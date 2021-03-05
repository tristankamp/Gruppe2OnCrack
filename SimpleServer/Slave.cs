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
        private readonly List<string> _dict = new List<string>();
        private string _password;
        private string _solvedPassword;
        private UserInfo _userInfo;
        private UserInfoClearText _result;

        public Slave()
        {
            _messageDigest = new SHA1CryptoServiceProvider();
            //_messageDigest = new MD5CryptoServiceProvider();
            // seems to be same speed
        }


        public void LavMitArbejde()
        {
            TcpClient ballAndChain = new TcpClient();

            ballAndChain.Connect(IPAddress.Loopback, 7);

            StreamWriter sw = new StreamWriter(ballAndChain.GetStream());
            StreamReader sr = new StreamReader(ballAndChain.GetStream());

            sw.AutoFlush = true;


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

                _password = sr.ReadLine();

                sw.WriteLine("Disconnect");
                ballAndChain.Close();
                ballAndChain = new TcpClient();
                


                if (_password == "fuck af")
                {
                    break;
                }

                string[] split = _password.Split(":");
                _userInfo = new UserInfo(split[0], split[1]);

                RunCracking();

                if (!(_result is null))
                {
                    _solvedPassword = _result.Password;
                    _result = null;
                }

                else _solvedPassword = "Kunne ikke";


                ballAndChain.Connect(IPAddress.Loopback, 7);
                sw = new StreamWriter(ballAndChain.GetStream());
                sr = new StreamReader(ballAndChain.GetStream());
                sw.AutoFlush = true;

                sw.WriteLine("2");
                sw.WriteLine(_password.Split(":")[0] + " " + _solvedPassword);




            }
            



        }








        /// <summary>
        /// Runs the password cracking algorithm
        /// </summary>
        public void RunCracking()
        {
          
            //foreach (string ord in dict)
            //{
            //    CheckWordWithVariations(ord, UI);

            //    if (!(_result is null))
            //    {
            //        break;
            //    }
            //}

            int count = 0;
            int length = _dict.Count;
            while (_result is null && count < length)
            {
                CheckWordWithVariations(_dict[count++], _userInfo);
            }

            //                                                              Console.WriteLine(string.Join(", ", result));
            //                                                              Console.WriteLine("Out of {0} password {1} was found ", userInfos.Count, result.Count);
            //                                                              Console.WriteLine();
            
        }

        /// <summary>
        /// Generates a lot of variations, encrypts each of the and compares it to all entries in the password file
        /// </summary>
        /// <param name="dictionaryEntry">A single word from the dictionary</param>
        /// <param name="userInfos">List of (username, encrypted password) pairs from the password file</param>
        /// <returns>A list of (username, readable password) pairs. The list might be empty</returns>
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
        /// Checks a single word (or rather a variation of a word): Encrypts and compares to all entries in the password file
        /// </summary>
        /// <param name="userInfos"></param>
        /// <param name="possiblePassword">List of (username, encrypted password) pairs from the password file</param>
        /// <returns>A list of (username, readable password) pairs. The list might be empty</returns>
        private void CheckSingleWord(UserInfo userInfo, String possiblePassword)
        {
            char[] charArray = possiblePassword.ToCharArray();
            byte[] passwordAsBytes = Array.ConvertAll(charArray, PasswordFileHandler.GetConverter());

            byte[] encryptedPassword = _messageDigest.ComputeHash(passwordAsBytes);
            //string encryptedPasswordBase64 = System.Convert.ToBase64String(encryptedPassword);

            //foreach (UserInfo userInfo in userInfos) //Decrypted passwords should be removed. Duplicate hash should be removed.
            //{
            //    if (CompareBytes(userInfo.EntryptedPassword, encryptedPassword))  //compares byte arrays
            //    {
            //        _result = new UserInfoClearText(userInfo.Username, possiblePassword);
            //        Console.WriteLine(userInfo.Username + " " + possiblePassword);
            //    }
            //}

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
            //if (secondArray == null)
            //{
            //    throw new ArgumentNullException("firstArray");
            //}
            //if (secondArray == null)
            //{
            //    throw new ArgumentNullException("secondArray");
            //}
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
