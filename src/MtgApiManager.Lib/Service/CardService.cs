using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MtgApiManager.Lib.Core;
using MtgApiManager.Lib.Dto;
using MtgApiManager.Lib.Model;
using MtgApiManager.Lib.Types;
using MtgApiManager.Lib.Utility;

namespace MtgApiManager.Lib.Service
{
    internal class CardService : ServiceBase<ICard>, ICardService
    {
        public CardService(
            IMtgApiServiceAdapter serviceAdapter,
            IModelMapper modelMapper,
            ApiVersion version,
            bool rateLimitOn = true)
            : base(serviceAdapter, modelMapper, version, ApiEndPoint.Cards, rateLimitOn)
        {
        }

        /// <inheritdoc />
        public async override Task<Exceptional<List<ICard>>> AllAsync()
        {
            try
            {
                var query = BuildUri(WhereQueries);
                var rootCardList = await CallWebServiceGet<RootCardListDto>(query).ConfigureAwait(false);

                return Exceptional<List<ICard>>.Success(MapCardsList(rootCardList), MtgApiController.CreatePagingInfo());
            }
            catch (Exception ex)
            {
                return Exceptional<List<ICard>>.Failure(ex);
            }
        }

        /// <inheritdoc />
        public Task<Exceptional<ICard>> FindAsync(int multiverseId) => FindAsync(multiverseId.ToString());

        /// <inheritdoc />
        public async Task<Exceptional<ICard>> FindAsync(string id)
        {
            try
            {
                var rootCard = await CallWebServiceGet<RootCardDto>(BuildUri(id)).ConfigureAwait(false);
                var model = ModelMapper.MapCard(rootCard.Card);

                return Exceptional<ICard>.Success(model, MtgApiController.CreatePagingInfo());
            }
            catch (Exception ex)
            {
                return Exceptional<ICard>.Failure(ex);
            }
        }

        /// <inheritdoc />
        public async Task<Exceptional<List<string>>> GetCardSubTypesAsync()
        {
            try
            {
                var url = new Uri(new Uri(BaseMtgUrl), string.Concat(Version.GetDescription(), "/", ApiEndPoint.CardSubTypes.GetDescription()));
                var rootTypeList = await CallWebServiceGet<RootCardSubTypeDto>(url).ConfigureAwait(false);

                return Exceptional<List<string>>.Success(rootTypeList.SubTypes, MtgApiController.CreatePagingInfo());
            }
            catch (Exception ex)
            {
                return Exceptional<List<string>>.Failure(ex);
            }
        }

        /// <inheritdoc />
        public async Task<Exceptional<List<string>>> GetCardSuperTypesAsync()
        {
            try
            {
                var url = new Uri(new Uri(BaseMtgUrl), string.Concat(Version.GetDescription(), "/", ApiEndPoint.CardSuperTypes.GetDescription()));
                var rootTypeList = await CallWebServiceGet<RootCardSuperTypeDto>(url).ConfigureAwait(false);

                return Exceptional<List<string>>.Success(rootTypeList.SuperTypes, MtgApiController.CreatePagingInfo());
            }
            catch (Exception ex)
            {
                return Exceptional<List<string>>.Failure(ex);
            }
        }

        /// <inheritdoc />
        public async Task<Exceptional<List<string>>> GetCardTypesAsync()
        {
            try
            {
                var url = new Uri(new Uri(BaseMtgUrl), string.Concat(Version.GetDescription(), "/", ApiEndPoint.CardTypes.GetDescription()));
                var rootTypeList = await CallWebServiceGet<RootCardTypeDto>(url).ConfigureAwait(false);

                return Exceptional<List<string>>.Success(rootTypeList.Types, MtgApiController.CreatePagingInfo());
            }
            catch (Exception ex)
            {
                return Exceptional<List<string>>.Failure(ex);
            }
        }

        /// <inheritdoc />        
        public ICardService WhereAnd(Expression<Func<CardQueryParameter, string>> property, params string[] values)
            => Where(property, values, Operator.AND);

        /// <inheritdoc />        
        public ICardService WhereOr(Expression<Func<CardQueryParameter, string>> property, params string[] values)
            => Where(property, values, Operator.OR);

        /// <inheritdoc />        
        public ICardService Where(Expression<Func<CardQueryParameter, string>> property, string value)
            => Where(property, new object[] { value }, Operator.AND);

        /// <inheritdoc />        
        public ICardService Where(Expression<Func<CardQueryParameter, int>> property, int value)
            => Where(property, new object[] { value }, Operator.AND);

        private ICardService Where<U>(Expression<Func<CardQueryParameter, U>> property, object[] values, Operator logicalOperator)
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
            var queryName = QueryUtility.GetQueryPropertyName<CardQueryParameter>(expression.Member.Name);
                        
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

        private List<ICard> MapCardsList(RootCardListDto cardListDto)
        {
            if (cardListDto == null)
            {
                throw new ArgumentNullException(nameof(cardListDto));
            }

            if (cardListDto.Cards == null)
            {
                return new List<ICard>();
            }

            return cardListDto.Cards
                .Select(x => ModelMapper.MapCard(x))
                .ToList();
        }
    }
}