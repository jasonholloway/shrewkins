using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Shrewkins.Test
{
    public class FlowificationTests
    {
        private readonly Graph Graph;

        public FlowificationTests() {
            Graph = new Graph();
        }
        
        public static IProgram Flowify(IEnumerable<Instruction> instructions) 
        {
            throw new NotImplementedException();
        }


        [Fact]
        public void Read_StaticNoArgs() 
        {
            var prog = Graph.ReadIlMethod(() => StaticNoArgs());
            
            prog.Inputs.ShouldBeEmpty();
            prog.Outputs.ShouldBeEmpty();
        }

        public static void StaticNoArgs() {
            Console.WriteLine("Hello!");
        }
        
        
        [Fact]
        public void Read_StaticWithArg() 
        {
            var prog = Graph.ReadIlMethod(() => StaticWithArg(13));
            
            prog.Inputs.ShouldHaveSingleItem();
            prog.Outputs.ShouldBeEmpty();
        }

        public static void StaticWithArg(int i) {
            Console.WriteLine("Hello "+ i);
        }
        
        
        
        
        
        [Fact]
        public void BlahBlahBlah()
        {
            var program = Graph.ReadIlMethod(() => Wibble(13));
            
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