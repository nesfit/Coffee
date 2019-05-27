using System;
using Barista.Common.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Common.OperationResults
{
    public static class Extensions
    {
        public static Exception ToException(this IOperationResult operationResult)
        {
            if (operationResult is null)
                throw new ArgumentNullException(nameof(operationResult));
            if (operationResult.Successful)
                throw new InvalidOperationException("The operation result is successful.");

            return new BaristaException(operationResult.ErrorCode, operationResult.ErrorMessage);
        }

        public static BadRequestObjectResult ToActionResult(this IOperationResult operationResult)
        {
            if (operationResult is null)
                throw new ArgumentNullException(nameof(operationResult));
            if (operationResult.Successful)
                throw new InvalidOperationException("The operation result is successful.");

            return new BadRequestObjectResult(new ErrorDto(operationResult.ErrorCode, operationResult.ErrorMessage));
        }
    }
}
