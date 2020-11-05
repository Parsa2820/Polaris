using Database.Exceptions.Elastic;
using Database.Filtering.Filter;
using Database.Validation.Elastic;
using Models;
using Models.Response;
using Nest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Database.Communication.Elastic.Nest
{
    public class NestElasticHandler<TModel> : IDatabaseHandler<TModel> where TModel : class, IModel
    {
        protected IElasticClient elasticClient;
        private static Dictionary<string, string> modelFilterMapping;

        public NestElasticHandler()
        {
            elasticClient = NestClientFactory.GetInstance().GetClient();
        }

        public void CheckSource(string sourceName, bool recreate = false)
        {
            try
            {
                ElasticIndexValidator.ValidateIndex(sourceName);
            }
            catch (InvalidElasticIndexException e)
            {
                if (recreate)
                {
                    elasticClient.Indices.Create(sourceName, i => i.Map<TModel>(x => x.AutoMap()));
                    // Todo Custom mapping
                }
                else
                    throw e;
            }
        }

        public void Insert(TModel model, string indexName)
        {
            CheckSource(indexName, false);
            var response = elasticClient.Index<TModel>(model, i => i.Index(indexName));
            ElasticResponseValidator.Validate(response);
        }

        public void BulkInsert(IEnumerable<TModel> models, string indexName)
        {
            CheckSource(indexName, true);
            var bulk = new BulkDescriptor();
            foreach (var model in models)
            {
                bulk.Index<TModel>(x => x.Index(indexName).Document(model));
            }
            var response = elasticClient.Bulk(bulk);
            ElasticResponseValidator.Validate(response);
        }

        public IEnumerable<TModel> FetchAll(string indexName)
        {
            return FetchAllByQuery(new MatchAllQuery(), indexName);
        }

        public IEnumerable<TModel> RetrieveQueryDocumentsByFilter(
            string[] filter,
            string indexName,
            Pagination pagination = null
        )
        {
            return RetrieveQueryDocuments(new NestFilter(filter, modelFilterMapping).Interpret(), indexName, pagination);
        }

        protected IEnumerable<TModel> RetrieveQueryDocuments(
            QueryContainer container,
            string indexName,
            Pagination pagination = null
        )
        {
            if (pagination != null)
            {
                var response = RetrieveQueryResponse(container, indexName, pagination);
                ElasticResponseValidator.Validate(response);
                return response.Documents;
            }
            return FetchAllByQuery(container, indexName);
        }


        protected ISearchResponse<TModel> RetrieveQueryResponse(
            QueryContainer container,
            string indexName,
            Pagination pagination
        )
        {
            var response = elasticClient.Search<TModel>(s => s
                .Index(indexName)
                .Query(q => container)
                .Size(pagination.PageSize)
                .From(pagination.PageIndex * pagination.PageSize));
            ElasticResponseValidator.Validate(response);
            return response;
        }

        protected IReadOnlyCollection<IHit<TModel>> RetrieveQueryHits(
            QueryContainer container,
            string indexName,
            Pagination pagination = null
        )
        {
            if (pagination != null)
            {
                var response = RetrieveQueryResponse(container, indexName, pagination);
                ElasticResponseValidator.Validate(response);
                return response.Hits;
            }
            return FetchAllHitsByQuery(container, indexName);
        }

        protected void DeleteById_(string id_, string indexName)
        {
            var response = elasticClient.Delete<TModel>(id_, dd => dd.Index(indexName));
            ElasticResponseValidator.Validate(response);
        }

        protected void UpdateById_(string id_, string indexName, TModel newModel)
        {
            var response = elasticClient.Update<TModel>(id_, u => u.Index(indexName).Doc(newModel));
            ElasticResponseValidator.Validate(response);
        }

        private IEnumerable<TModel> FetchAllByQuery(QueryContainer queryContainer, string indexName)
        {
            var response = NestScrollSearchInit(queryContainer, indexName);
            var result = new List<TModel>();
            var anyHitsLeft = true;
            string scrollId = response.ScrollId;
            while (anyHitsLeft)
            {
                if (response.IsValid)
                {
                    result.AddRange(response.Documents);
                    scrollId = response.ScrollId;
                    response = elasticClient.Scroll<TModel>("2m", scrollId);
                    ElasticResponseValidator.Validate(response);
                }
                anyHitsLeft = response.Documents.Any();
            }
            elasticClient.ClearScroll(new ClearScrollRequest(scrollId));
            return result;
        }

        private ISearchResponse<TModel> NestScrollSearchInit(
            QueryContainer container,
            string indexName,
            string scrollTimeout = "2m",
            int scrollSize = 10000
        )
        {
            var response = elasticClient.Search<TModel>(s => s
                .Index(indexName)
                .From(0)
                .Take(scrollSize)
                .Query(q => container)
                .Scroll(scrollTimeout)
            );
            ElasticResponseValidator.Validate(response);
            return response;
        }

        private IReadOnlyCollection<IHit<TModel>> FetchAllHitsByQuery(QueryContainer container, string indexName)
        {
            var response = NestScrollSearchInit(container, indexName);
            var result = new List<IHit<TModel>>();
            var anyHitsLeft = true;
            string scrollId = response.ScrollId;
            while (anyHitsLeft)
            {
                if (response.IsValid)
                {
                    result.AddRange(response.Hits);
                    scrollId = response.ScrollId;
                    response = elasticClient.Scroll<TModel>("2m", scrollId);
                    ElasticResponseValidator.Validate(response);
                }
                anyHitsLeft = response.Hits.Any();
            }
            elasticClient.ClearScroll(new ClearScrollRequest(scrollId));
            return new ReadOnlyCollection<IHit<TModel>>(result);
        }

        private void InitModelFilterMapping()
        {
            var properties = typeof(TModel).GetProperties();
            modelFilterMapping = new Dictionary<string, string>();

            foreach (var property in properties)
                modelFilterMapping.Add(property.Name.ToLower(), GetFilterType(property.PropertyType));
        }

        private string GetFilterType(Type type)
        {
            if (type.Equals(typeof(int)) || type.Equals(typeof(long)) || type.Equals(typeof(short)))
                return "numeric";
            else
                return "text";
        }
    }
}