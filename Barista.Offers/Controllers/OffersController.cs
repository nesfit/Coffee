using System;
using System.Threading.Tasks;
using Barista.Common.AspNetCore;
using Barista.Common.Dispatchers;
using Barista.Contracts;
using Barista.Offers.Dto;
using Barista.Offers.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Offers.Controllers
{
    [Route("api/offers")]
    [ApiController]
    public class OffersController : BaristaController
    {
        public OffersController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<IPagedResult<OfferDto>> BrowseOffers([FromQuery] BrowseOffers query)
        {
            return await QueryAsync(query);
        }

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<OfferDto>> GetOffer(Guid id)
        {
            var offer = await QueryAsync(new GetOffer(id));
            if (offer is null)
                return NotFound();

            return offer;
        }
    }
}