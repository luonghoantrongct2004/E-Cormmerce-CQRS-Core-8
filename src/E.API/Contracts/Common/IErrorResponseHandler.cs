﻿namespace E.API.Contracts.Common;

public interface IErrorResponseHandler
{
    ErrorResponse HandleErrors(List<Error> errors);
}