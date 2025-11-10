using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akinator
{
    public class Node
    {
        public string Value { get; set; }
        public Node Parent { get; set; }
        public Node False { get; set; }
        public Node True { get; set; }
        public bool IsQuestion
        {
            get 
            {
                return Value.EndsWith("?") ? true : false;
            }
        }

        public Node(string value) 
        {
            Value = value;
        }

    }
}
