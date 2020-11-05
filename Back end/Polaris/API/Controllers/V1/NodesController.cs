using API.Services.NodeBusiness;
using Database.Exceptions;
using Microsoft.AspNetCore.Mvc;

using Models.Banking;
using Models.Network;
using Models.Response;


namespace API.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class NodesController : ControllerBase
    {
        private readonly INodeService<BankAccount, string> _nodeService;

        public NodesController(INodeService<BankAccount, string> nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpGet]
        [Route("typing")]
        public IActionResult GetTyping()
        {
            return Ok("Some fields");
        }

        [HttpGet]
        [Route("{nodeId}")]
        public IActionResult GetNodeById(string nodeId)
        {
            Node<BankAccount, string> node;
            try
            {
                node = _nodeService.GetNodeById(nodeId);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound($"NodeNotFound: {e.Message}");
            }
            return Ok(node);
        }

        [HttpDelete]
        [Route("{nodeId}")]
        public IActionResult DeleteNodeById(string nodeId)
        {
            try
            {
                _nodeService.DeleteNodeById(nodeId);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound($"NodeNotFound: {e.Message}");
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult GetNodesByFilter([FromQuery] string[] filter, [FromQuery] Pagination pagination)
        {
            if (pagination != null && pagination.PageIndex == 0 && pagination.PageSize == 0)
                return Ok(_nodeService.GetNodesByFilter(filter, null));

            return Ok(_nodeService.GetNodesByFilter(filter, pagination));
        }

        [HttpPost]
        public IActionResult AddNewNode([FromBody] Node<BankAccount, string> node)
        {
            _nodeService.InsertNode(node);
            return Created($"api/v1/nodes/{node.Id}", node);
        }

        [HttpPut]
        public IActionResult UpdateExistingNode([FromBody] Node<BankAccount, string> newNode)
        {
            try
            {
                _nodeService.UpdateNode(newNode);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound($"NodeNotFound: {e.Message}");
            }
            return Ok();
        }
    }
}