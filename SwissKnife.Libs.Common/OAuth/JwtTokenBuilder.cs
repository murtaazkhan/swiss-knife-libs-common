using Microsoft.IdentityModel.Tokens;
using SwissKnife.Libs.Common.OAuth.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SwissKnife.Libs.Common.OAuth;

/// <summary>
/// Contains Jwt Token Builder
/// </summary>
public sealed class JwtTokenBuilder
{
    private SecurityKey securityKey = null;
    private string subject = "";
    private string issuer = "";
    private string audience = "";
    private string emailId = "";
    private readonly Dictionary<string, string> claims = new();
    private List<string> roles = new();
    private int expiryInMinutes = 5;

    /// <summary>
    /// This extension adds security key to Jwt Token Builder
    /// </summary>
    /// <param name="securityKey"></param>
    /// <returns></returns>
    public JwtTokenBuilder AddSecurityKey(SecurityKey securityKey)
    {
        this.securityKey = securityKey;
        return this;
    }

    /// <summary>
    /// This extension adds security key to Jwt Token Builder
    /// </summary>
    /// <param name="signingKey"></param>
    /// <returns></returns>
    public JwtTokenBuilder AddSecurityKey(string signingKey)
    {
        this.securityKey =  CreateSecurityKey(signingKey);
        return this;
    }

    /// <summary>
    /// This extension adds subject to Jwt Token Builder
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public JwtTokenBuilder AddSubject(string subject)
    {
        this.subject = subject;
        return this;
    }

    /// <summary>
    /// This extension adds issuer to Jwt Token Builder
    /// </summary>
    /// <param name="issuer"></param>
    /// <returns></returns>
    public JwtTokenBuilder AddIssuer(string issuer)
    {
        this.issuer = issuer;
        return this;
    }

    /// <summary>
    /// This extension adds audience to Jwt Token Builder
    /// </summary>
    /// <param name="audience"></param>
    /// <returns></returns>
    public JwtTokenBuilder AddAudience(string audience)
    {
        this.audience = audience;
        return this;
    }

    /// <summary>
    /// This extension adds email id to Jwt Token Builder
    /// </summary>
    /// <param name="emailId"></param>
    /// <returns></returns>
    public JwtTokenBuilder AddEmailId(string emailId)
    {
        this.emailId = emailId;
        return this;
    }

    /// <summary>
    /// This extension adds claim key to Jwt Token Builder
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public JwtTokenBuilder AddClaim(string type, string value)
    {
        claims.Add(type, value);
        return this;
    }

    /// <summary>
    /// This extension add claims to Jwt Token Builder
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    public JwtTokenBuilder AddClaims(Dictionary<string, string> claims)
    {
        this.claims.Union(claims);
        return this;
    }

    /// <summary>
    /// This extension adds roles to Jwt Token Builder
    /// </summary>
    /// <param name="roles"></param>
    /// <returns></returns>
    public JwtTokenBuilder AddRoles(List<string> roles)
    {
        this.roles = roles;
        return this;
    }

    /// <summary>
    /// This extension adds expiry to Jwt Token Builder
    /// </summary>
    /// <param name="expiryInMinutes"></param>
    /// <returns></returns>
    public JwtTokenBuilder AddExpiry(int expiryInMinutes)
    {
        this.expiryInMinutes = expiryInMinutes;
        return this;
    }

    /// <summary>
    /// This method builds Jwt Token
    /// </summary>
    /// <returns></returns>
    public JwtToken Build()
    {
        EnsureArguments();

        var claims = new List<Claim>
        {
            // Unique Id for all Jwt tokes
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // Issuer
            new(JwtRegisteredClaimNames.Iss, issuer),
            // Issued at
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            //
            new(JwtRegisteredClaimNames.Sub, subject),
            // to invalidate the cookie
            new(JwtRegisteredClaimNames.Email, emailId),
        }
        .Union(this.claims.Select(item => new Claim(item.Key, item.Value)))
        .Union(roles.Select(item => new Claim(ClaimTypes.Role, item)));

        var token = new JwtSecurityToken(
                            issuer: issuer,
                            audience: audience,
                            claims: claims,
                            expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

        return new JwtToken(token);
    }

    #region Private Methods

    /// <summary>
    /// This method validates the required arguments for Jwt Token Builder
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    private void EnsureArguments()
    {
        if (securityKey == null)
            throw new ArgumentNullException("Security Key");

        if (string.IsNullOrEmpty(subject))
            throw new ArgumentNullException("Subject");

        if (string.IsNullOrEmpty(issuer))
            throw new ArgumentNullException("Issuer");

        if (string.IsNullOrEmpty(audience))
            throw new ArgumentNullException("Audience");

        if (string.IsNullOrEmpty(emailId))
            throw new ArgumentNullException("Email Id");
    }

    private static SymmetricSecurityKey CreateSecurityKey(string secret)
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
    }
    #endregion
}
