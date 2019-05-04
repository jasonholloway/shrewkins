using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ICSharpCode.Decompiler.CSharp.Syntax;
using JsonLens.Compiler;
using Shouldly;
using Xunit;
using Xunit.Abstractions;
using static Shrewkins.OpMarkers;
using static Shrewkins.Helpers;
using OpCode = System.Reflection.Emit.OpCode;


namespace Shrewkins
{
    public abstract class Jump
    {
        public static Jump Here<M>() where M : Jump, new() {
            throw new NotImplementedException();
        }
    }

    public static class OpMarkers
    {
        public static Jump Jump<T>() where T : IChunk, new() 
        {
            throw new NotImplementedException();
        }
    }



    public static class Helpers
    {
        public static void AssertThat(Func<bool> test) {
            if(!test()) throw new ApplicationException();
        }
    }
    
    
    public static class Vars
    {
        public static ref Readable<char> Input => throw new NotImplementedException();

        public static Stack<int> SomeStack => default;

        public static ref Context Context => throw new NotImplementedException();
        
        public static ref int Cursor => throw new NotImplementedException();
    }

    
    public interface IChunk
    {
    }

    interface ITemplateChunk : IChunk
    {
        IDictionary<string, object> Constants { get; }
    }

    interface IEmitChunk : IChunk
    {
        ISet<Type> Ins { get; } 
        ISet<Type> Outs { get; }
        void Emit(object o);
    }
    

    public class ReadChar : ITemplateChunk
    {
        public IDictionary<string, object> Constants { get; }
        
        public ReadChar(int someConstant)
        {
            Constants = new Dictionary<string, object> {
                { nameof(SomeConstant), someConstant },
                { nameof(AnotherConstant), "WibbleWibbleWibble!" }
            };
        }

        static ref int SomeConstant => throw new NotImplementedException();
        static string AnotherConstant => default;
        
        static Jump Template() 
        {
            var some = SomeConstant;
            var another = AnotherConstant;
            
            var inp = Vars.Input;
            var cursor = Vars.Cursor;

            switch (inp.Peek) {
                case 'a':
                    return Jump<SwitchOnChar>();
                case (char)0x23:
                    throw new Exception("hello");
            }
            
            
            if (cursor > 10) {
                cursor++;
                return Jump<SwitchOnChar>();
            }
            
            var krrumpt = $"{some} and {another}!";
            
            return Jump<SwitchOnChar>();
        }

    }
    
    public class SwitchOnChar : IEmitChunk
    {
        public ISet<Type> Ins { get; }
        public ISet<Type> Outs { get; }
        
        public void Emit(object o) 
        {
            throw new NotImplementedException();
        }
    }

    
    public class FindProps : IEmitChunk
    {
        readonly ISet<string> _propNames;
        
        public FindProps(ISet<string> propNames) {
            _propNames = propNames;
        }

        public ISet<Type> Ins { get; }
        public ISet<Type> Outs { get; }
        
        public void Emit(object o) 
        {
            throw new NotImplementedException();
        }
    }
    

    public sealed class MyJump : Jump
    { }



    public class ModuleSource : MetadataSource
    {
        public readonly Module Module;
        
        public ModuleSource(Module module) {
            Module = module;
        }
    }
    

    public class UnitTest1
    {
        readonly ITestOutputHelper _output;

        public UnitTest1(ITestOutputHelper output) {
            _output = output;
        }


        [Fact]
        public void Pointless1() {
            var type = typeof(UnitTest1);
            var method = type.GetMethod("Plop", BindingFlags.Static | BindingFlags.NonPublic).GetMethodBody();
            
            var els = Reader.Read(type.Module, method, method.GetILAsByteArray());
            
            _output.WriteLine(OpPrinter.Print(els));
        }

        public static void Plop() {
            string plop = "plopplopplop";

            if (plop.Length > 10) {
                Console.WriteLine(plop + "wooooo!");
            }
        }

        static void Mooo() {
            string moo = "moomoomoo";

            if (moo.Length < 1234) {
                Console.WriteLine(moo + "wooooo!");
            }
        }


        

        public static LinkedList<Instruction> ReadStaticMethod(Expression<Action> exp) 
        {
            var method = ((MethodCallExpression)exp.Body).Method;
            var body = method.GetMethodBody();
            return Reader.Read(method.Module, body, body.GetILAsByteArray());
        }
        

        [Fact]
        public void Conjoining() 
        {
            var prog1 = ReadStaticMethod(() => Mooo());
            var prog2 = ReadStaticMethod(() => Plop());
            
            var conjoined = new LinkedList<Instruction>(prog1.Concat(prog2));
            
            _output.WriteLine(OpPrinter.Print(conjoined));
        }


        [Fact]
        public void Test1() 
        {
            //let's analyse a template
            var tOp = typeof(ReadChar);
            var mTemplate = tOp.GetMethod("Template", BindingFlags.Static | BindingFlags.NonPublic);
            AssertThat(() => !mTemplate.GetParameters().Any());
            AssertThat(() => mTemplate.ReturnType == typeof(Jump));

            var body = mTemplate.GetMethodBody();
            var els = Reader.Read(tOp.Module, body, body.GetILAsByteArray());
            
            //1) parse into linked list of ops
            //2) insert labels
            //3) print out

            var sb = new StringBuilder();

            foreach (var el in els) {
                switch (el) {
                    case BasicInstruction i:
                        sb.AppendLine($"[{i.Op.Value:x4}]\t{i.Op.Name.PadRight(10)}\t{i.Operand}");
                        break;
                }
            }
            
            _output.WriteLine(sb.ToString());
        }
        
        
        [Fact]
        public void Regen_LocalStaticMethods() 
        {
            var prog = ReadStaticMethod(() => CallsLocalStaticMethod());
            _output.WriteLine(OpPrinter.Print(prog));

            Verifier.Verify(prog);

            var generated = Generator.Generate(prog);

            generated.Invoke(null, new object[0]);
        }

        public static void CallsLocalStaticMethod() {
            Plop();
        }


        [Fact]
        public void Regen_WithLocal() 
        {
            var prog = ReadStaticMethod(() => ParpyParpParp());
            _output.WriteLine(OpPrinter.Print(prog));

            var generated = Generator.Generate(prog);

            generated.Invoke(null, new object[0]);
        }

        public static void ParpyParpParp() {
            var a = 1;
            var b = 2;
            var c = 3;
            var d = 4;
            var e = 5;
            b = a;
            c = b;
            d = c;
            e = d;
            a = e;
        }
        
        
        [Fact]
        public void Regen_Simplest() 
        {
            var prog = ReadStaticMethod(() => Krrumpt());

            var generated = Generator.Generate(prog.Cast<BasicInstruction>());

            generated.Invoke(null, new object[0]);
        }

        public static void Krrumpt() {
            return;
        }
        
        
    }


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