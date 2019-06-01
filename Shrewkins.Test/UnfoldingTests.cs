using System;
using System.Linq;
using Xunit;
using static Shrewkins.Test.Helpers;

namespace Shrewkins.Test
{
    public class UnfoldingTests
    {


        [Fact]
        public void Wibble() {
            var m = ReadIlMethod(() => Hello());

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