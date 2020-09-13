// In The Name Of GOD
using Analysis.GraphStructure.Structures;
using Elastic.Models;
using System.Collections.Generic;
using System.Linq;

namespace Analysis.GraphStructure
{
    public class Graph<NID, NDATA, EID, EDATA> : IGraph<NID, NDATA, EID, EDATA>
    where NDATA : Entity<NID>
    where EDATA : Entity<EID>
    {
        public Dictionary<Node<NID, NDATA>, LinkedList<Edge<EID, EDATA, Node<NID, NDATA>>>> Adj { get; set; }
        public Dictionary<NID, Node<NID, NDATA>> IDToNode { get; set; }
        public Dictionary<NDATA, Node<NID, NDATA>> DataToNode { get; set; }
        public List<Node<NID, NDATA>> GetNeighbors(NDATA data)
        {
            var node = DataToNode[data];
            return ReadNeighbors(node);
        }

        public Graph()
        {
            foreach (var item in Adj)
            {
                IDToNode[item.Key.Id] = item.Key;
                DataToNode[item.Key.Data] = item.Key;
            }
        }

        private List<Node<NID, NDATA>> ReadNeighbors(Node<NID, NDATA> node)
        {
            var set = new HashSet<Node<NID, NDATA>>();
            foreach (var edge in Adj[node])
            {
                set.Add(edge.Target);
            }

            return set.ToList();
        }

        public List<Node<NID, NDATA>> GetNeighbors(NID id)
        {
            var node = IDToNode[id];
            return ReadNeighbors(node);
        }
    }
}