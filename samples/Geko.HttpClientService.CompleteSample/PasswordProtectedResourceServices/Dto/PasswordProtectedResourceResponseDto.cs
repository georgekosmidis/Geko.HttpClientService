﻿namespace Geko.HttpClientService.CompleteSample.PasswordProtectedResourceServices.Dto;

/// <summary>
/// An object representing the result of the protected resource https://demo.identityserver.io/api/test
/// </summary>
public record class PasswordProtectedResourceResponseDto
{
    /// <summary>
    /// The type property of the result
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// The value property of the result
    /// </summary>
    public string? Value { get; set; }
}