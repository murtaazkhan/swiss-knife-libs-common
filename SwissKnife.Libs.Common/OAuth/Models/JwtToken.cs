﻿using System.IdentityModel.Tokens.Jwt;

namespace SwissKnife.Libs.Common.OAuth.Models;

public sealed class JwtToken
{
    private readonly JwtSecurityToken token;

    internal JwtToken(JwtSecurityToken token)
    {
        this.token = token;
    }

    public DateTime ValidTo => token.ValidTo;
    public string Value => new JwtSecurityTokenHandler().WriteToken(this.token);
}
