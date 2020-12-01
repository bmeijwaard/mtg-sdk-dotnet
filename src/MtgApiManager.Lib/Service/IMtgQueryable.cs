namespace MtgApiManager.Lib.Service
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines an object to be query able against the MTG API.
    /// </summary>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <typeparam name="TQuery">The type of query.</typeparam>
    public interface IMtgQueryable<TService, TQuery>
        where TQuery : IQueryParameter
    {
        /// <summary>
        /// Adds a query parameter for multiple search parameters using the AND operator.
        /// </summary>
        /// <param name="property">The property to add the query for.</param>
        /// <param name="values">The values of the query.</param>
        /// <returns>The instance of its self with the new query parameter.</returns>
        TService WhereAnd(Expression<Func<TQuery, string>> property, params string[] values);

        /// <summary>
        /// Adds a query parameter for multiple search parameters using the OR operator.
        /// </summary>
        /// <param name="property">The property to add the query for.</param>
        /// <param name="values">The values of the query.</param>
        /// <returns>The instance of its self with the new query parameter.</returns>
        TService WhereOr(Expression<Func<TQuery, string>> property, params string[] values);

        /// <summary>
        /// Adds a query parameter for multiple search parameters using a single string value
        /// </summary>        
        /// <param name="property">The property to add the query for.</param>
        /// <param name="value">The value of the query.</param>
        /// <returns>The instance of its self with the new query parameter.</returns>
        TService Where(Expression<Func<TQuery, string>> property, string value);

        /// <summary>
        /// Adds a query parameter for multiple search parameters using a single integer value
        /// </summary>        
        /// <param name="property">The property to add the query for.</param>
        /// <param name="value">The value of the query.</param>
        /// <returns>The instance of its self with the new query parameter.</returns>
        TService Where(Expression<Func<TQuery, int>> property, int value);
    }
}