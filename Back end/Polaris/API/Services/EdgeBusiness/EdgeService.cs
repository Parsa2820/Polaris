using Database.Communication;
using Database.Filtering;
using Microsoft.Extensions.Configuration;
using Models;
using Models.Network;
using Models.Response;
using Nest;
using System.Collections.Generic;
using System.Linq;

namespace API.Services.EdgeBusiness
{
    public class EdgeService<TDataModel, TTypeDataId, TTypeSideId> : IEdgeService<TDataModel, TTypeDataId, TTypeSideId>
    where TDataModel : AmountedEntity<TTypeDataId, TTypeSideId>
    {
        private readonly IEntityHandler<TDataModel, TTypeDataId> _handler;
        private readonly string _edgeElasticIndexName;

        public EdgeService(IConfiguration config, IEntityHandler<TDataModel, TTypeDataId> handler)
        {
            _handler = handler;
            _edgeElasticIndexName = config["TransactionsIndexName"];
        }

        public Edge<TDataModel, TTypeDataId, TTypeSideId> GetEdgeById(TTypeDataId id)
        {
            return new Edge<TDataModel, TTypeDataId, TTypeSideId>(_handler.GetEntity(id, _edgeElasticIndexName));
        }

        public IEnumerable<Edge<TDataModel, TTypeDataId, TTypeSideId>> GetEdgesById(TTypeDataId[] ids)
        {
            return _handler.GetEntities(ids, _edgeElasticIndexName).Select(
                entity => new Edge<TDataModel, TTypeDataId, TTypeSideId>(entity)
            );
        }

        public void DeleteEdgeById(TTypeDataId id)
        {
            _handler.DeleteEntity(id, _edgeElasticIndexName);
        }

        public void InsertEdge(Edge<TDataModel, TTypeDataId, TTypeSideId> edge)
        {
            _handler.Insert(edge.Data, _edgeElasticIndexName);
        }

        public void UpdateEdge(Edge<TDataModel, TTypeDataId, TTypeSideId> newEdge)
        {
            _handler.UpdateEntity(newEdge.Data, _edgeElasticIndexName);
        }

        public IEnumerable<Edge<TDataModel, TTypeDataId, TTypeSideId>> GetEdgesByFilter(
            string[] filter = null,
            Pagination pagination = null
        )
        {
            var data = _handler.RetrieveQueryDocumentsByFilter(
                filter,
                _edgeElasticIndexName,
                pagination
            );
            var output = data.Select(d => new Edge<TDataModel, TTypeDataId, TTypeSideId>(d));
            return output;
        }

        private Dictionary<string, string> GetModelMapping()
        {
            return new Dictionary<string, string>{{"id", "text"}, {"source", "text"}, {"target", "text"},
                {"amount", "numeric"}, {"date", "numeric"}, {"time", "numeric"},
                {"trackingId", "numeric"}, {"type", "text"}};
        }

        public IEnumerable<Edge<TDataModel, TTypeDataId, TTypeSideId>> GetEdgesBySideId(
            TTypeSideId id,
            string[] filter = null,
            Pagination pagination = null
        )
        {
            return GetEdgesBySourceId(id, filter, pagination).Union(GetEdgesByTargetId(id, filter, pagination));
        }

        public IEnumerable<Edge<TDataModel, TTypeDataId, TTypeSideId>> GetEdgesBySideIds(
            TTypeSideId[] ids,
            string[] filter = null,
            Pagination pagination = null
        )
        {
            return GetEdgesBySourceIds(ids, filter, pagination).Union(GetEdgesByTargetIds(ids, filter, pagination));
        }

        public IEnumerable<Edge<TDataModel, TTypeDataId, TTypeSideId>> GetEdgesBySourceId(
            TTypeSideId id,
            string[] filter = null,
            Pagination pagination = null
        )
        {
            if (filter is null)
                filter = new string[] { };

            var filters = filter.ToList();
            filters.Add($"source eq {id}");
            return GetEdgesByFilter(filters.ToArray(), pagination);
        }

        public IEnumerable<Edge<TDataModel, TTypeDataId, TTypeSideId>> GetEdgesBySourceIds(
            TTypeSideId[] ids,
            string[] filter = null,
            Pagination pagination = null
        )
        {
            if (filter is null)
                filter = new string[] { };

            var filters = filter.ToList();
            filters.Add($"source eq {string.Join(" ", ids.Select(i => i.ToString()))}");
            return GetEdgesByFilter(filters.ToArray(), pagination);
        }

        public IEnumerable<Edge<TDataModel, TTypeDataId, TTypeSideId>> GetEdgesByTargetId(
            TTypeSideId id,
            string[] filter = null,
            Pagination pagination = null
        )
        {
            if (filter is null)
                filter = new string[] { };
            var filters = filter.ToList();
            filters.Add($"target eq {id}");
            return GetEdgesByFilter(filters.ToArray(), pagination);
        }

        public IEnumerable<Edge<TDataModel, TTypeDataId, TTypeSideId>> GetEdgesByTargetIds(
            TTypeSideId[] ids,
            string[] filter = null,
            Pagination pagination = null
        )
        {
            if (filter is null)
                filter = new string[] { };

            var filters = filter.ToList();
            filters.Add($"target eq {string.Join(" ", ids.Select(i => i.ToString()))}");
            return GetEdgesByFilter(filters.ToArray(), pagination);
        }
    }
}