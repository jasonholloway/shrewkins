using System;
using System.Collections.Generic;

namespace Shrewkins
{
    public static class Verifier
    {
        public static void Verify(IEnumerable<Instruction> ins) {
            foreach (var @in in ins) {
                switch (@in) {
                    case MethodCallInstruction i: {
                        if(!i.Method.IsPublic) throw new InvalidOperationException("Can't call private non-local methods!");
                        break;
                    }
                }
            }
        }
            
    }
}