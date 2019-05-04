using System.Reflection;
using System.Reflection.Emit;

namespace Shrewkins
{
    public abstract class Instruction 
    {
    }

    public sealed class BasicInstruction : Instruction
    {
        public OpCode Op { get; }
        public Operand Operand { get; }
        
        public BasicInstruction(OpCode op, Operand operand) {
            Op = op;
            Operand = operand;
        }
    }

    
    public abstract class VarInstruction : Instruction
    {
        public LocalVariableInfo Var { get; }
        
        protected VarInstruction(LocalVariableInfo @var) {
            Var = @var;
        }
    }

    public sealed class StoreVarInstruction : VarInstruction
    {
        public StoreVarInstruction(LocalVariableInfo @var) 
            : base(@var) { }
    }
    
    public sealed class LoadVarInstruction : VarInstruction
    {
        public LoadVarInstruction(LocalVariableInfo @var) 
            : base(@var) { }
    }


    public sealed class MethodCallInstruction : Instruction
    {
        public MethodInfo Method { get; }
        public bool IsVirtualDispatch { get; }

        public MethodCallInstruction(MethodInfo method, bool isVirtualDispatch) {
            Method = method;
            IsVirtualDispatch = isVirtualDispatch;
        }
    }
    
    

    public class StringOperand : MetadataOperand
    {
        public StringOperand(MetadataSource source, int token)
            : base(source, token) { }
    }

    public class MethodOperand : MetadataOperand
    {
        public MethodOperand(MetadataSource source, int token)
            : base(source, token) { }
    }
    
    public class TypeOperand : MetadataOperand
    {
        public TypeOperand(MetadataSource source, int token)
            : base(source, token) { }
    }

    public abstract class MetadataOperand : Operand
    {
        public object Source => null;
        public int Token => (int)Raw;

        protected MetadataOperand(MetadataSource source, int token)
            : base(4, token) { }
    }

    public class Operand
    {
        public readonly int Size;
        public readonly long Raw;
            
        public Operand(int size, long raw) {
            Raw = raw;
            Size = size;
        }

        public override string ToString()
            => Size == 0 
                ? "."
                : $"<{Raw.ToString($"x{Size}")}>";
    }
}