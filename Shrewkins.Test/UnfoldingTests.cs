using System;
using System.Linq;
using Xunit;

namespace Shrewkins.Test
{
    public class UnfoldingTests
    {
        private Graph Graph = new Graph();


        [Fact]
        public void Wibble() {
            var m = Graph.ReadIlMethod(() => Hello());
        }


        public static void Hello() {
            Console.WriteLine("Hello!");
        }
    }

    public class Unfolding
    {
        public ISource[] Ins;
        public Scenario[] Scenarios;
    }

    public class Scenario
    {
        public ITarget[] Outs;
    }
    
    public class Operation
    {
        public ISource[] Ins;
        public Unfolding Unfolding;
    }
}