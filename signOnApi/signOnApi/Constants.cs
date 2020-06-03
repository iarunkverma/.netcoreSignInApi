using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signOnApi
{
    public static class Constants
    {
        public static string Audience = "http://localhost:49587/";
        public static string Issuer = "Issuer";
        public static string SecertKey = "this_is_secret_key_can_be_any_string_but_it_need_to_be_so_long_otherwise_it_gives_error";
    }
}
