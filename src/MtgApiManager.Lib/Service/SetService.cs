using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MtgApiManager.Lib.Core;
using MtgApiManager.Lib.Dto;
using MtgApiManager.Lib.Dto.Set;
using MtgApiManager.Lib.Model;
using MtgApiManager.Lib.Types;
using MtgApiManager.Lib.Utility;

namespace MtgApiManager.Lib.Service
{
    internal class SetService : ServiceBase<ISet>, ISetService
    {
        public SetService(
            IMtgApiServiceAdapter serviceAdapter,
            IModelMapper modelMapper,
            ApiVersion version,
            bool rateLimitOn = true)
            : base(serviceAdapter, modelMapper, version, ApiEndPoint.Sets, rateLimitOn)
        {
        }

        /// <inheritdoc />
        public async override Task<Exceptional<List<ISet>>> AllAsync()
        {
            try
            {
                var query = BuildUri(WhereQueries);
                var rootSetList = await CallWebServiceGet<RootSetListDto>(query).ConfigureAwait(false);
                return Exceptional<List<ISet>>.Success(MapSetsList(rootSetList), MtgApiController.CreatePagingInfo());
            }
            catch (Exception ex)
            {
                return Exceptional<List<ISet>>.Failure(ex);
            }
        }

        /// <inheritdoc />
        public async Task<Exceptional<ISet>> FindAsync(string code)
        {
            try
            {
                var rootSet = await CallWebServiceGet<RootSetDto>(BuildUri(code)).ConfigureAwait(false);
                var model = ModelMapper.MapSet(rootSet.Set);

                return Exceptional<ISet>.Success(model, MtgApiController.CreatePagingInfo());
            }
            catch (Exception ex)
            {
                return Exceptional<ISet>.Failure(ex);
            }
        }

        /// <inheritdoc />
        public async Task<Exceptional<List<ICard>>> GenerateBoosterAsync(string code)
        {
            try
            {
                var url = new Uri(Path.Combine(BuildUri(code).AbsoluteUri, "booster"));
                var rootCardList = await CallWebServiceGet<RootCardListDto>(url).ConfigureAwait(false);

                var cards = rootCardList.Cards
                .Select(x => ModelMapper.MapCard(x))
                .ToList();

                return Exceptional<List<ICard>>.Success(cards, MtgApiController.CreatePagingInfo());
            }
            catch (Exception ex)
            {
                return Exceptional<List<ICard>>.Failure(ex);
            }
        }

        /// <inheritdoc />
        public ISetService WhereAnd(Expression<Func<SetQueryParameter, string>> property, params string[] values)
            => Where(property, values, Operator.AND);

        /// <inheritdoc />
        public ISetService WhereOr(Expression<Func<SetQueryParameter, string>> property, params string[] values)
            => Where(property, values, Operator.OR);

        /// <inheritdoc />
        public ISetService Where(Expression<Func<SetQueryParameter, string>> property, string value)
            => Where(property, new object[] { value }, Operator.AND);

        /// <inheritdoc />
        public ISetService Where(Expression<Func<SetQueryParameter, int>> property, int value)
            => Where(property, new object[] { value }, Operator.AND);


        private ISetService Where<U>(Expression<Func<SetQueryParameter, U>> property, object[] values, Operator logicalOperator)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (values.Any(v => EqualityComparer<object>.Default.Equals(v, default)))
            {
                throw new ArgumentNullException(nameof(values));
            }

            MemberExpression expression = property.Body as MemberExpression;
            var queryName = QueryUtility.GetQueryPropertyName<SetQueryParameter>(expression.Member.Name);

            if (values.Length > 1)
            {
                var queryOperator = logicalOperator.GetOperator();
                WhereQueries[queryName] = string.Join(queryOperator, values);
            }
            else
            {
                WhereQueries[queryName] = Uri.UnescapeDataString(Convert.ToString(values[0]));
            }

            return this;
        }

        private List<ISet> MapSetsList(RootSetListDto setListDto)
        {
            if (setListDto == null)
            {
                throw new ArgumentNullException(nameof(setListDto));
            }

            if (setListDto.Sets == null)
            {
                return new List<ISet>();
            }

            return setListDto.Sets
                .Select(x => ModelMapper.MapSet(x))
                .ToList();
        }
    }
}