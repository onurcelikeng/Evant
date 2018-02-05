using System;

namespace Evant.DAL.EF.Tables
{
    public class UserSetting
    {
        public Guid UserSettingId { get; set; }

        public string Theme { get; set; } = "light";

        public string Language { get; set; } = "tr";

        public bool IsFriendshipNotif { get; set; } = true;

        public bool IsCommentNotif { get; set; } = true;

        public bool IsEventNewComerNotif { get; set; } = true;

        public bool IsEventUpdateNotif { get; set; } = true;


        // Foreign keys
        public virtual User User { get; set; }
    }
}
