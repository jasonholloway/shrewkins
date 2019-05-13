using System;
using System.Collections.Generic;
using System.Xml;
using Shouldly;
using Xunit;
using static Shrewkins.Test.Helpers;

namespace Shrewkins.Test
{
    public class FlowificationTests
    {
        public static IProgram Flowify(IEnumerable<Instruction> instructions) 
        {
            throw new NotImplementedException();
        }


        [Fact]
        public void Read_StaticNoArgs() 
        {
            var prog = ReadIlMethod(() => StaticNoArgs());
            prog.Inputs.ShouldBeEmpty();
            prog.Outputs.ShouldBeEmpty();
        }

        public static void StaticNoArgs() {
            Console.WriteLine("Hello!");
        }
        
        
        [Fact]
        public void Read_StaticWithArg() 
        {
            var prog = ReadIlMethod(() => StaticWithArg(13));
            
            prog.Inputs.ShouldHaveSingleItem();
            prog.Outputs.ShouldBeEmpty();
        }

        public static void StaticWithArg(int i) {
            Console.WriteLine("Hello "+ i);
        }
        
        
        
        
        
        [Fact]
        public void BlahBlahBlah()
        {
            var program = ReadIlMethod(() => Wibble(13));
            
        }

        
        public static void Wibble(int a) 
        {
            var i = 13 + a;

            if (i > 10) {
                Console.WriteLine($"Hello! {13 + i}");
            }
        }
        
        
        
        
        
        
    }
}