namespace SwissKnife.Libs.Common.Audit.Models
{
    /// <summary>
    /// Contains Http Request Audit.
    /// </summary>
    public class HttpRequestAuditModel
    {
        /// <summary>
        /// Gets or Sets the Audit Timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Gets or Sets the Client Ip Address.
        /// </summary>
        public string ClientIpAddress { get; set; }
        /// <summary>
        /// Gets or Sets the Client Source.
        /// </summary>
        public string ClientSource { get; set; }
        /// <summary>
        /// Gets or Sets the Method Type.
        /// </summary>
        public string MethodType { get; set; }
        /// <summary>
        /// Gets or Sets the Request Url.
        /// </summary>
        public string RequestUrl { get; set; }
        /// <summary>
        /// Gets or Sets the Authorization object from request.
        /// </summary>
        public AuthorizationModel Authorization { get; set; }
        /// <summary>
        /// Gets or Sets the Request Status.
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Gets or Sets the Requested Resource.
        /// </summary>
        public string RequestedResource { get; set; }
        /// <summary>
        /// Gets or Sets the Description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or Sets the Affected Entity.
        /// </summary>
        public string AffectedEntity { get; set; }
    }

    public class AuthorizationModel
    {
        /// <summary>
        /// Gets or Sets the Name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or Sets the Username.
        /// </summary>
        public string Username{ get; set; }
        /// <summary>
        /// Gets or Sets the Roles.
        /// </summary>
        public string Roles { get; set; }
        /// <summary>
        /// Gets or Sets the IpAddress.
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// Gets or Sets the TenantId.
        /// </summary>
        public string TenantId { get; set; }
        /// <summary>
        /// Gets or Sets the Issuer.
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// Gets or Sets the App Id.
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// Gets or Sets the Audience.
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// Gets or Sets the Scope.
        /// </summary>
        public string Scope { get; set; }
    }
}
