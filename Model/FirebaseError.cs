using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteClone.Model
{
    public class ErrorDetails
    {
        public int code {  get; set; }
        public string message { get; set; }
    }

    public class FirebaseError
    {
        public ErrorDetails error {  get; set; }
    }
}
