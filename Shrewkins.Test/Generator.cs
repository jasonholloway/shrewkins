using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using ICSharpCode.Decompiler.IL;
using OpCode = System.Reflection.Emit.OpCode;

namespace Shrewkins
{
    public static class Generator
    {

        public static MethodInfo Generate(IEnumerable<Instruction> ins) 
        {
            var type = AssemblyBuilder
                .DefineDynamicAssembly(new AssemblyName("assembly"), AssemblyBuilderAccess.Run)
                .DefineDynamicModule("module")
                .DefineType("Machine");

            var method = type.DefineMethod("Method", MethodAttributes.Public | MethodAttributes.Static);
            
            var il = method.GetILGenerator();

            var vars = ins.OfType<VarInstruction>()
                .Select(i => i.Var) 
                .Distinct()
                .ToDictionary(v => v.LocalIndex, v => il.DeclareLocal(v.LocalType))
                .ToArray();
            
            foreach (var @in in ins) {
                switch (@in) {
                    case BasicInstruction i: {
                        var op = i.Op;
                        var operand = i.Operand;
                        switch (i.Operand.Size) {
                            case 0:
                                il.Emit(op);
                                break;
                            case 1:
                                il.Emit(op, (byte)operand.Raw);
                                break;
                            case 2:
                                il.Emit(op, (short)operand.Raw);
                                break;
                            case 4:
                                il.Emit(op, (int)operand.Raw);
                                break;
                            case 8:
                                il.Emit(op, (long)operand.Raw);
                                break;
                            default:
                                throw new NotImplementedException("Strange operand size!");
                        }
                        break;
                    }

                    case StoreVarInstruction i: {
                        il.Emit(OpCodes.Stloc_S, (short)i.Var.LocalIndex);    //could be much improved obvs
                        break;
                    }
                    
                    case LoadVarInstruction i: {
                        il.Emit(OpCodes.Ldloc_S, (short)i.Var.LocalIndex);    //could be much improved obvs
                        break;
                    }

                    case MethodCallInstruction i: {
                        if (i.IsVirtualDispatch) {
                            throw new NotImplementedException();
                        }
                        else {
                            il.EmitCall(OpCodes.Call, i.Method, new Type[0]);
                        }
                        break;
                    }
                    
                    default:
                        throw new NotImplementedException($"Instruction {@in} not handled!");
                }
            }
                
            return type.CreateType()
                .GetMethod("Method");
        }
            
    }
}