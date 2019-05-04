using System.Collections.Generic;
using System.IO;

namespace Shrewkins
{
    public static class OpPrinter
    {
        public static void Print(TextWriter writer, IEnumerable<Instruction> els) {
            foreach(var el in els) Print(writer, el);
        }

        public static void Print(TextWriter writer, Instruction el) {
            switch (el) {
                case BasicInstruction i:
                    writer.WriteLine($"[{i.Op.Value:x4}]\t{i.Op.Name.PadRight(10)}\t{i.Operand}");
                    break;
                case StoreVarInstruction i:
                    writer.WriteLine($"StoreVar #{i.Var.LocalIndex} ({i.Var.LocalType})");
                    break;
                case MethodCallInstruction i:
                    writer.WriteLine($"Call {i.Method.Name}");
                    break;
                default:
                    writer.WriteLine($"{el}");
                    break;
            }
        }
            
        public static string Print(IEnumerable<Instruction> els) {
            using (var writer = new StringWriter()) {
                Print(writer, els);
                return writer.ToString();
            }
        }
    }
}