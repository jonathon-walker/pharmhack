﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pharmhack.OData
{
	public static class ExpressionExtensions
	{
		public static T ReplaceParameter<T>(this T expression, ParameterExpression oldParam, ParameterExpression newParam)
			where T : Expression
		{
			return new ReplaceVisitor(oldParam, newParam).VisitAndConvert(expression, "ExpressionExtensions.ReplaceParameter`1");
		}

		public static Expression<Func<T1, TResult>> Chain<T1, T2, TResult>(this Expression<Func<T1, T2>> a, Expression<Func<T2, TResult>> b)
		{
			return Expression.Lambda<Func<T1, TResult>>(
					new ReplaceVisitor(b.Parameters.Single(), a.Body).VisitAndConvert(b.Body, "ExpressionExtensions.Chain`3"),
					a.Parameters);
		}

		public static Expression<Func<T1, bool>> In<T1, T2>(this Expression<Func<T1, T2>> member, params T2[] values)
		{
			var prop = member.Body as MemberExpression;
			if (prop == null)
				throw new Exception("Expression has to be member");
			if (values.Length == 0)
				return _ => true;
			var body = values.Select(v => Expression.Equal(prop, Expression.Constant(v))).Aggregate(Expression.OrElse);
			return Expression.Lambda<Func<T1, bool>>(body, member.Parameters[0]);
		}

		/// <summary>
		/// http://stackoverflow.com/questions/19433316/combine-two-linq-lambda-expressions
		/// </summary>
		class ReplaceVisitor : ExpressionVisitor
		{
			readonly Expression from;
			readonly Expression to;

			public ReplaceVisitor(Expression from, Expression to)
			{
				this.from = from;
				this.to = to;
			}

			public override Expression Visit(Expression node)
			{
				return node == from ? to : base.Visit(node);
			}
		}
	}
}
