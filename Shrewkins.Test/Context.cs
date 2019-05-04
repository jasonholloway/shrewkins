using System;
using System.Collections.Generic;

namespace JsonLens.Compiler
{
    public ref struct Context 
    {
        public readonly Stack<Machine> Stack;
        public readonly Readable<char> In;

        public Context(int i) {
            In = new Readable<char>();
            Stack = new Stack<Machine>();
        }
        
        //bindables please...!!
        //se
        //
        //
        
        
        
    }
}