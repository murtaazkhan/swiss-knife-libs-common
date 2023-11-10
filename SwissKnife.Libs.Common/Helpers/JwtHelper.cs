
using System.IdentityModel.Tokens.Jwt;

namespace SwissKnife.Libs.Common.Helpers
{
    /// <summary>
    /// This class provided Jwt helper functions.
    /// </summary>
    public static class JwtHelper
    {
        /// <summary>
        /// To provide value for respective claim type from Jwt token.
        /// </summary>
        /// <param name="authToken">Jwt authorization token</param>
        /// <param name="claimType">claim type for which value needs to be returned </param>
        public static string GetClaimValueFromToken(string authToken, string claimType)
        {
            string result = null;

            authToken = authToken.Replace("Bearer ", "");

            var canReadToken = new JwtSecurityTokenHandler().CanReadToken(authToken);
            if (canReadToken)
            {
                var tokenDecode = new JwtSecurityTokenHandler().ReadToken(authToken) as JwtSecurityToken;
                var claimValue = tokenDecode?.Claims?.FirstOrDefault(c => c.Type == claimType);
                result = claimValue?.Value;
            }

            return result;
        }
    }
}
