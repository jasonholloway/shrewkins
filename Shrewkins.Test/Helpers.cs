using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Shrewkins.Test
{
    internal static class Helpers
    {
        public static LinkedList<Instruction> ReadStaticMethod(Expression<Action> exp) 
        {
            var method = ((MethodCallExpression)exp.Body).Method;
            var body = method.GetMethodBody();
            return Reader.Read(method.Module, body, body.GetILAsByteArray());
        }

        public static IlMethod ReadIlMethod(Expression<Action> exp)
        {
            var method = ((MethodCallExpression)exp.Body).Method;
            var body = method.GetMethodBody();
            return Reader.ReadMethod(method.Module, body, body.GetILAsByteArray());
        }
    }
}