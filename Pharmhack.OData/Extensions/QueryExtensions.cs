using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

		public static IEnumerable<T> WhereBetweenDates<T>(this IEnumerable<T> source, Expression<Func<T, DateTime>> dateSelector, DateTime from, DateTime to)
			where T : class
		{
			return source.AsQueryable().WhereBetweenDates(dateSelector, from, to);
		}
	}
}
