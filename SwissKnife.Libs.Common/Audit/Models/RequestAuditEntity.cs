using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwissKnife.Libs.Common.Audit.Models;

/// <summary>
/// Contains the Request Audit Entity.
/// </summary>
[Table("audits")]
public class RequestAuditEntity : AuditEntity
{
    /// <summary>
    /// Gets or Sets the Database Unique Key.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or Sets the Request Timestamp.
    /// </summary>
    [Column("timestamp")]
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// Gets or Sets the Client Ip Address.
    /// </summary>
    [Column("clientIpAddress")]
    public string ClientIpAddress { get; set; }

    /// <summary>
    /// Gets or Sets the Client Source.
    /// </summary>
    [Column("clientSource")]
    public string ClientSource { get; set; }

    /// <summary>
    /// Gets or Sets the Http Request Method Type.
    /// </summary>
    [Column("methodType")]
    public string MethodType { get; set; }

    /// <summary>
    /// Gets or Sets the Http Request Url.
    /// </summary>
    [Column("requestUrl")]
    public string RequestUrl { get; set; }

    /// <summary>
    /// Gets or Sets the Requester Name.
    /// </summary>
    [Column("requesterName")]
    public string RequesterName { get; set; }

    /// <summary>
    /// Gets or Sets the Requester Username.
    /// </summary>
    [Column("requesterUsername")]
    public string RequesterUsername { get; set; }

    /// <summary>
    /// Gets or Sets the Http Request Authorization payload.
    /// </summary>
    [Column("authorizationPayload")]
    public string AuthorizationPayload { get; set; }

    /// <summary>
    /// Gets or Sets the Http Response status.
    /// </summary>
    [Column("status")]
    public string Status { get; set; }

    /// <summary>
    /// Gets or Sets the Requested Resource.
    /// </summary>
    [Column("requestedResource")]
    public string RequestedResource { get; set; }

    /// <summary>
    /// Gets or Sets the Description.
    /// </summary>
    [Column("description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or Sets the Affected Entity.
    /// </summary>
    [Column("affectedEntity")]
    public string AffectedEntity { get; set; }
}

