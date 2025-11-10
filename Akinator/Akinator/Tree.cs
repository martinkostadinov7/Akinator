using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akinator
{
    public class Tree
    {
        public Node Root {  get; set; }
        public Tree(string value) 
        {
            Root = new Node(value);
        }
    }
}