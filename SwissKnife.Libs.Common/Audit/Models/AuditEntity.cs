using System.ComponentModel.DataAnnotations.Schema;

namespace SwissKnife.Libs.Common.Audit.Models
{
    /// <summary>
    /// Contains Audit Entity.
    /// </summary>
    public class AuditEntity
    {
        /// <summary>
        /// Gets or Sets the Created By.
        /// </summary>
        [Column("createdBy")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets the Modified By.
        /// </summary>
        [Column("modifiedBy")]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets the Created Date Time.
        /// </summary>
        [Column("createdDateTime")]
        public DateTimeOffset? CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or Sets the Modified Date Time.
        /// </summary>
        [Column("modifiedDateTime")]
        public DateTimeOffset? ModifiedDateTime { get; set; }
    }
}
