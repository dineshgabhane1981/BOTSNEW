namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class BOTS_TblDiscussion
    {
        public int Id { get; set; }

        [Required]
        [StringLength(4)]
        public string GroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string SpokenTo { get; set; }

        [Required]
        [StringLength(50)]
        public string ContactNo { get; set; }

        [Required]
        [StringLength(50)]
        public string CallMode { get; set; }

        public int CallType { get; set; }

        [StringLength(50)]
        public string SubCallType { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FollowupDate { get; set; }

        public string Description { get; set; }

        public string ActionItems { get; set; }

        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }        

        [Required]
        [StringLength(250)]
        public string AddedBy { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [NotMapped]
        public string GroupName { get; set; }

        public string Department { get; set; }
        
        public string AssignedMember { get; set; }
        
        public string Priority { get; set; }
        
        public string DiscussionType { get; set; }
        public string AttachedFile { get; set; }
        public string DiscussionDoneNotDone { get; set; }

        [NotMapped]
        public string ReassignMember { get; set; }
    }

    public class DiscussionDetails
    {
        public string GroupName { get; set; }
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }
        public string Addeddt { get; set; }
        public string SpokenTo { get; set; }
        public string ContactNo { get; set; }
        public string CustomerType { get; set; }
        public string CallType { get; set; }
        public string SubCallType { get; set; }
        public string Status { get; set; }
        public DateTime? FollowupDate { get; set; }
        public string Followupdt { get; set; }
        public string CallMode { get; set; }
        public string Description { get; set; }
        public string ActionItems { get; set; }
        public string AddedBy { get; set; }
        public string AssignedMember { get; set; }
        public int SubDiscussionCount { get; set; }
        public string DiscussionType { get; set; }
        public string Priority { get; set; }
        public string DoneNotDone { get; set; }


    }

    public class EmailDetails
    {
        public string DepartHead { get; set; }
        public string Addby { get; set; }
        public string SendTo { get; set; }
        public string Priority { get; set; }
        public string Member { get; set; }
        public string CallTypetext { get; set; }
        public string subtypetext { get; set; }
        public string GroupName { get; set; }
        public int id { get; set; }
        public string Description { get; set; } 
        public string FilePath { get; set; }
        public string TeamName { get; set; }
        public string AddbyEmail { get; set; }
        public string MemberCompleted { get; set; }
        public string DepartHeadName { get; set; }
        public string FromName { get; set; }
        public string DiscussionAddedBy { get; set; }

    }

    public class MessageDetails
    {
        public string Mobileno { get; set; }
        public string Message { get; set; }
        public string Tokenid { get; set; }
        public string Groupid { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public string TeamName { get; set; }
        public string SpokenTo { get; set; }
        public string Addby { get; set; }
        public string BOEmpName { get; set; }
       
    }

    public class UpdateCompleteThread
    {
        public string dId { get; set; }
        public string Desc { get; set; }
        public string Status { get; set; }
        public string FollowupDate { get; set; }
        public string Reassign { get; set; }
        public string FileName { get; set; }
        public string File { get; set; }
        public string RequestType { get; set; }
        public string DoneNotDone { get; set; }


    }
}
