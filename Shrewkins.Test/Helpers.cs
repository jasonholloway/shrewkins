using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Shrewkins.Test
{
    public static class ReaderExtensions
    {
        public static LinkedList<Instruction> ReadStaticMethod(Expression<Action> exp) 
        {
            var method = ((MethodCallExpression)exp.Body).Method;
            var body = method.GetMethodBody();
            return Reader.Read(method.Module, body, body.GetILAsByteArray());
        }

        public static Program ReadIlMethod(this Graph graph, Expression<Action> exp)
        {
            var method = ((MethodCallExpression)exp.Body).Method;
            return Reader.ReadMethod(graph, method);
        }
    }
}