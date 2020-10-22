using Database.Communication;
using Database.Communication.Elastic.Nest;
using Database.Filtering;
using Microsoft.Extensions.Configuration;
using Models;
using Models.Network;
using Models.Response;
using System.Collections.Generic;
using System.Linq;

namespace API.Services.NodeBusiness
{
    public class NodeService<TDataModel, TTypeDataId> : INodeService<TDataModel, TTypeDataId>
    where TDataModel : Entity<TTypeDataId>
    {
        private readonly IEntityHandler<TDataModel, TTypeDataId> _handler;
        private readonly string _nodeElasticIndexName;

        public NodeService(IConfiguration config, IEntityHandler<TDataModel, TTypeDataId> handler)
        {
            _nodeElasticIndexName = config["AccountsIndexName"];
            _handler = handler;
        }

        public Node<TDataModel, TTypeDataId> GetNodeById(TTypeDataId id)
        {
            return new Node<TDataModel, TTypeDataId>(_handler.GetEntity(id, _nodeElasticIndexName));
        }

        public IEnumerable<Node<TDataModel, TTypeDataId>> GetNodesById(TTypeDataId[] ids)
        {
            return _handler.GetEntities(ids, _nodeElasticIndexName).Select(
                entity => new Node<TDataModel, TTypeDataId>(entity)
            );
        }

        public void DeleteNodeById(TTypeDataId id)
        {
            _handler.DeleteEntity(id, _nodeElasticIndexName);
        }

        public void InsertNode(Node<TDataModel, TTypeDataId> node)
        {
            _handler.Insert(node.Data, _nodeElasticIndexName);
        }

        public void UpdateNode(Node<TDataModel, TTypeDataId> newNode)
        {
            _handler.UpdateEntity(newNode.Data, _nodeElasticIndexName);
        }

        public IEnumerable<Node<TDataModel, TTypeDataId>> GetNodesByFilter(
            string[] filter = null,
            Pagination pagination = null
        )
        {
            var data = _handler.RetrieveQueryDocumentsByFilter(
                filter,
                _nodeElasticIndexName,
                pagination
            );
            return data.Select(d => new Node<TDataModel, TTypeDataId>(d));
        }

        private Dictionary<string, string> GetModelMapping()
        {
            return new Dictionary<string, string>{{"id", "text"}, {"cardId", "text"}, {"sheba", "text"},
                {"accountType", "text"}, {"branchTelephone", "text"}, {"branchAdress", "text"}, {"branchName", "text"},
                {"ownerName", "text"}, {"ownerFamilyName", "text"}, {"ownerId", "text"}, {"ownerPrimaryName", "text"}};
        }
    }
}