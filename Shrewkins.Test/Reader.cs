using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using JsonLens.Compiler;

namespace Shrewkins
{
    public static class Reader
    {
        static IReadOnlyDictionary<short, OpCode> _opCodes
            = typeof(OpCodes)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => (OpCode)f.GetValue(null))
                .ToDictionary(o => o.Value);
        
        
        public static LinkedList<Instruction> Read(Module module, MethodBody method, ReadOnlySpan<byte> il) 
        {
            var els = new LinkedList<Instruction>();

            while (!il.IsEmpty) {
                var (bytesRead, instr) = ReadInstruction(module, method, il);
                els.AddLast(instr);
                il = il.Slice(bytesRead);
            }

            return els;
        }
        
        
        public static (int, Instruction) ReadInstruction(Module module, MethodBody method, ReadOnlySpan<byte> il) 
        {
            var b = il[0];
            var byteCode = (short)(b == 0xFE ? (b << 8) + il[1] : b);
            var opCode = _opCodes[byteCode];
                
            var operand = ReadOperand(il.Slice(opCode.Size));

            var size = opCode.Size + operand.Size;

            return (size, ReadVar()
                            ?? ReadCall()
                            ?? ReadBasic());

            Instruction ReadCall() {
                switch ((OpCodeValues) opCode.Value) {
                    case OpCodeValues.Ldftn:
                    case OpCodeValues.Ldvirtftn:
                        throw new NotImplementedException();
                    
                    case OpCodeValues.Jmp:
                        throw new NotImplementedException();
                    
                    case OpCodeValues.Call:
                        return new MethodCallInstruction((MethodInfo)module.ResolveMethod((int)operand.Raw), false);
                    
                    case OpCodeValues.Callvirt:
                        return new MethodCallInstruction((MethodInfo)module.ResolveMethod((int)operand.Raw), true);
                    
                    default:
                        return null;
                }
            }

            Instruction ReadVar() {
                switch ((OpCodeValues) opCode.Value) {
                    case OpCodeValues.Ldloc:
                        throw new NotImplementedException();

                    case OpCodeValues.Ldloc_0:
                        return new LoadVarInstruction(method.LocalVariables[0]);

                    case OpCodeValues.Ldloc_1:
                        return new LoadVarInstruction(method.LocalVariables[1]);

                    case OpCodeValues.Ldloc_2:
                        return new LoadVarInstruction(method.LocalVariables[2]);

                    case OpCodeValues.Ldloc_3:
                        return new LoadVarInstruction(method.LocalVariables[3]);

                    case OpCodeValues.Ldloc_S:
                        return new LoadVarInstruction(method.LocalVariables[(int) operand.Raw]);

                    case OpCodeValues.Ldloca:
                    case OpCodeValues.Ldloca_S:
                        throw new NotImplementedException();

                    case OpCodeValues.Stloc:
                        throw new NotImplementedException();

                    case OpCodeValues.Stloc_0:
                        return new StoreVarInstruction(method.LocalVariables[0]);
                    case OpCodeValues.Stloc_1:
                        return new StoreVarInstruction(method.LocalVariables[1]);
                    case OpCodeValues.Stloc_2:
                        return new StoreVarInstruction(method.LocalVariables[2]);
                    case OpCodeValues.Stloc_3:
                        return new StoreVarInstruction(method.LocalVariables[3]);
                    case OpCodeValues.Stloc_S:
                        return new StoreVarInstruction(method.LocalVariables[(int) operand.Raw]);

                    default:
                        return null;
                }
            }

            Instruction ReadBasic() {
                return new BasicInstruction(opCode, operand);
            }

            Operand ReadOperand(ReadOnlySpan<byte> _il) {
                switch (opCode.OperandType) {
                    case OperandType.InlineNone:
                        return new Operand(0, 0);

                    case OperandType.InlineMethod: {
                        return new MethodOperand(null, Decode32(_il));
                    }

                    case OperandType.InlineType: {
                        return new TypeOperand(null, Decode32(_il));
                    }

                    case OperandType.InlineString: {
                        return new StringOperand(null, Decode32(_il));
                    }
                        
                    case OperandType.InlineBrTarget:
                    case OperandType.InlineI:
                    case OperandType.InlineSig:
                    case OperandType.InlineSwitch:
                    case OperandType.InlineTok:
                    case OperandType.ShortInlineR:
                    case OperandType.InlineField: {
                        return new Operand(4, Decode32(_il));
                    }

                    case OperandType.ShortInlineI:
                    case OperandType.ShortInlineBrTarget:
                    case OperandType.ShortInlineVar: {
                        return new Operand(1, _il[0]);
                    }

                    case OperandType.InlineI8:
                    case OperandType.InlineR: {
                        return new Operand(8, Decode64(_il));
                    }

                    case OperandType.InlineVar: {
                        return new Operand(2, Decode16(_il));
                    }

                    default:
                        throw new NotImplementedException();
                }
            }

            short Decode16(ReadOnlySpan<byte> @in)
                => BitConverter.ToInt16(@in);
                
            int Decode32(ReadOnlySpan<byte> @in)
                => BitConverter.ToInt32(@in);
                
            long Decode64(ReadOnlySpan<byte> @in)
                => BitConverter.ToInt64(@in);
        }
        
    }
    
}