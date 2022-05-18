﻿namespace StockService.Entity.Options;

public class AuthTokenOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecurityKey { get; set; }
}