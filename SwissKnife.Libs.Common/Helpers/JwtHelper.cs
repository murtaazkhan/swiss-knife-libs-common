using System.IdentityModel.Tokens.Jwt;

namespace SwissKnife.Libs.Common.Helpers;

/// <summary>
/// This class provided Jwt helper functions.
/// </summary>
public static class JwtHelper
{
    /// <summary>
    /// To provide value for respective claim type from Jwt token.
    /// </summary>
    /// <param name="jwtToken">Jwt authorization token</param>
    /// <param name="claimType">claim type for which value needs to be returned </param>
    public static string GetClaimValueFromToken(string jwtToken, string claimType)
    {
        string result = null;

        jwtToken = jwtToken.Replace("Bearer ", "");

        var canReadToken = new JwtSecurityTokenHandler().CanReadToken(jwtToken);
        if (canReadToken)
        {
            var tokenDecode = new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
            var claimValue = tokenDecode?.Claims?.FirstOrDefault(c => c.Type == claimType);
            result = claimValue?.Value;
        }

        return result;
    }
}

