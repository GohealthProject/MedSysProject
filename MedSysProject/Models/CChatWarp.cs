﻿namespace MedSysProject.Models
{
    public class CChatWarp
    {
        public int MessageId { get; set; }

        public int? RoomId { get; set; }

        public int? MemberId { get; set; }

        public int? EmployeeId { get; set; }

        public string MessageContent { get; set; }

        public bool? ReadState { get; set; }

        public DateTime? SendTime { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Member Member { get; set; }

        public virtual Room Room { get; set; }
    }
}
