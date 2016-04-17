using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pharmhack.OData
{
	public static class QueryExtensions
	{
		public static IQueryable<T> WhereBetweenDates<T>(this IQueryable<T> source, Expression<Func<T, DateTime>> dateSelector, DateTime from, DateTime to)
			where T : class
		{
			return source.Where(dateSelector.Chain(x => x >= from && x < to));
		}

		public static IQueryable<T> WhereBetweenDates<T>(this IQueryable<T> source, Expression<Func<T, DateTimeOffset>> dateSelector, DateTime from, DateTime to)
			where T : class
		{
			return source.Where(dateSelector.Chain(x => x >= from && x < to));
		}

		public static IEnumerable<T> WhereBetweenDates<T>(this IEnumerable<T> source, Expression<Func<T, DateTime>> dateSelector, DateTime from, DateTime to)
			where T : class
		{
			return source.AsQueryable().WhereBetweenDates(dateSelector, from, to);
		}

		public static IEnumerable<T> WhereBetweenDates<T>(this IEnumerable<T> source, Expression<Func<T, DateTimeOffset>> dateSelector, DateTime from, DateTime to)
			where T : class
		{
			return source.AsQueryable().WhereBetweenDates(dateSelector, from, to);
		}

		public static IQueryable<T1> WhereIn<T1, T2>(this IQueryable<T1> source, Expression<Func<T1, T2>> member, params T2[] values)
		{
			return source.Where(member.In(values));
		}

		public static IEnumerable<T1> WhereIn<T1, T2>(this IEnumerable<T1> source, Expression<Func<T1, T2>> member, params T2[] values)
		{
			return source.AsQueryable().WhereIn(member, values);
		}
	}
}
