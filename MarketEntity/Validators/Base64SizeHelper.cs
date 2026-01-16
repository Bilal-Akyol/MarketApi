using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Validators
{
    public static class Base64SizeHelper
    {
        // Base64 string -> byte size hesaplar
        public static long GetBytesFromBase64(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64)) return 0;

            // padding say
            int padding = 0;
            if (base64.EndsWith("==")) padding = 2;
            else if (base64.EndsWith("=")) padding = 1;


            // Base64 uzunluğundan byte hesap
            long bytes = (base64.Length * 3L) / 4L - padding;
            return bytes;
        }
    }
}
