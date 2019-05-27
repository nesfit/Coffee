using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Api.Dto;
using Barista.Api.ResourceAuthorization;
using Barista.Common;
using Barista.Common.Dto;
using Barista.Common.OperationResults;
using Barista.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IBusPublisher Publisher { get; }

        public BaseController(IBusPublisher busPublisher)
        {
            Publisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        protected async Task<TResult> SendRequest<TCommand, TResult>(TCommand command) where TCommand : class, ICommand where TResult : class, IOperationResult
        {
            return await Publisher.SendRequest<TCommand, TResult>(command);
        }

        protected async Task<ActionResult> SendAndHandleOperationCommand<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var result = await SendRequest<TCommand, IOperationResult>(command);
            if (!result.Successful)
                return result.ToActionResult();

            return NoContent();
        }

        private static object DefaultRouteParamsFunc(Guid id) => new {id};

        protected async Task<ActionResult> SendAndHandleIdentifierResultCommand<TCommand>(TCommand command, string actionName, Func<Guid, object> routeParamsFunc = null)
            where TCommand : class, ICommand
        {
            var result = await SendRequest<TCommand, IIdentifierResult>(command);
            if (!result.Successful)
                return result.ToActionResult();

            return CreatedAtAction(actionName, (routeParamsFunc ?? DefaultRouteParamsFunc)(result.Id.Value), new
            {
                id = result.Id.Value
            });
        }

        protected async Task<ActionResult> StartLongRunningOperation<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var result = await SendRequest<TCommand, ILongRunningOperationResult>(command);
            if (!result.Successful)
                return result.ToActionResult();

            Response.Headers.Add("X-Operation", result.LongRunningOperationId.ToString());
            return Accepted();
        }

        protected async Task<ActionResult> StartLongRunningOperation<TCommand>(TCommand command, Guid id) where TCommand : class, ICommand
        {
            var result = await SendRequest<TCommand, ILongRunningOperationResult>(command);
            if (!result.Successful)
                return result.ToActionResult();

            Response.Headers.Add("X-Operation", result.LongRunningOperationId.ToString());
            return Accepted(new {id});
        }

        protected ActionResult<T> Single<T>(T data)
        {
            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        protected ActionResult<IPagedResult<T>> Collection<T>(IPagedResult<T> pagedResult)
        {
            if (pagedResult == null)
            {
                return NotFound();
            }

            return Ok(pagedResult);
        }
    }
}
