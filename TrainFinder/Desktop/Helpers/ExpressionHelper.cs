using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Desktop
{
    public static class ExpressionHelper
    {
        public static T GetPropertyValue<T>(this Expression<Func<T>> lambada)
        {
            return lambada.Compile().Invoke();
        }

        public static void SetPropertyValue<T>(this Expression<Func<T>> lambada,T value)
        {
            //convert a lambada 
            var expression = (lambada as LambdaExpression).Body as MemberExpression;

            var PropertyInfor = (PropertyInfo) expression.Member;
            var target = Expression.Lambda(expression.Expression).Compile().DynamicInvoke();

            PropertyInfor.SetValue(target,value);


        }
}
}
