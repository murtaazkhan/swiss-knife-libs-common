namespace SwissKnife.Libs.Common.OAuth.Models;

public class TokenMetaData
{
    public string Name { get; set; }
    public string UserName { get; set; }
    public string UserId { get; set; }
    public string UserEmailId { get; set; }
    public List<string> UserRoles { get; set; }
    public int AccessTokenExpirationMinutes { get; set; }
}
