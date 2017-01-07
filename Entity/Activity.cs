using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;

namespace Entity
{
    public class Activity : IEntity
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string ActivityType { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime ActivityDateTime { get; set; }
        public int? RelevantCompanyID { get; set; }
        /// <summary>
        /// Represents object bound to activity (eg. InternshipID, MessageID....)
        /// </summary>
        public int ObjectID { get; set; }

        #region Virtual properties

        [ForeignKey("RelevantCompanyID")]
        public Company RelevantCompany { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }


        #endregion

        #region IEntity members

        public object GetObjectID()
        {
            return ID;
        }
        public string GetCodeName()
        {
            return ID.ToString();
        }

        #endregion

    }
}
