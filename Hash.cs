using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace FS19ModManager
{
    internal class Hash
    {
        HashAlgorithm algorithm;

        public Hash()
        {
            algorithm = MD5.Create();
        }

        public string? hash(string file)
        {
            return algorithm.ComputeHash(File.ReadAllBytes(file)).ToString();
        }
        public string? hash(string file, string name)
        {
            return algorithm.ComputeHash(File.ReadAllBytes(file).Concat(UTF8Encoding.UTF8.GetBytes(name)).ToArray()).ToString();
        }
    }
}
