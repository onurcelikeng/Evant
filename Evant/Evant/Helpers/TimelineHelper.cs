using Evant.DAL.EF.Tables;
using System;
using System.Collections.Generic;

namespace Evant.Helpers
{
    public static class TimelineHelper
    {
        public static string GenerateCommentBody(Guid userId, Comment comment)
        {
            if (userId == comment.Event.UserId)
            {
                return "Etkinliğine yorum bıraktın: " + comment.Content;
            }
            else
            {
                return "Yeni yorum bıraktın: " + comment.Content;
            }
        }

        public static string GenerateFollowingBody()
        {
            var contents = new List<string>()
            {
                "Çevren genişlemeye başladı bile, yeni birini takip ettin.",
                "Sosyalleşmeye başladın sanırım.",
                "O da seni takip ediyor mu acaba?"
            };

            int index = new Random().Next(0, contents.Count);
            return contents[index];
        }

        public static string GenerateFollowerBody()
        {
            var contents = new List<string>()
            {
                "Bugün yine popülersin, yeni bir takipçi kazandın!",
                "Takipçi sayın bir tık arttı.",
                "Seni takip eden birisi var."
            };

            int index = new Random().Next(0, contents.Count);
            return contents[index];
        }

        public static string GenerateCreateEventBody(Event @event)
        {
            int index = new Random().Next(0, 100);

            //Category
            if (index % 3 == 0)
            {
                return @event.Category.Name + " kategorisinde bir etkinlik oluşturdun.";
            }
            else if (index % 3 == 1)
            {
                return @event.City + "'de yeni bir etkinlik oluşturdun";
            }
            else
            {
                TimeSpan span = @event.FinishDate - @event.StartDate;
                if (span.Days == 0)
                {
                    return span.Hours + " saat sürecek bir etkinlik oluşturdun.";
                }
                else
                {
                    return span.Days + " gün" +  span.Hours + " saat sürecek bir etkinlik oluşturdun.";
                }
            }
        }

        public static string GenerateJoinEventBody(Event @event)
        {
            int index = new Random().Next(0, 100);

            //Category
            if (index % 3 == 0)
            {
                return @event.Category.Name + " kategorisinde bir etkinliğe katıldın.";
            }
            else if (index % 3 == 1)
            {
                return @event.City + "'de yeni bir etkinliğe katıldın.";
            }
            else
            {
                TimeSpan span = @event.FinishDate - @event.StartDate;
                if (span.Days == 0)
                {
                    return span.Hours + " saat sürecek bir etkinliğe katıldın.";
                }
                else
                {
                    return span.Days + " gün" + span.Hours + " saat sürecek bir etkinliğe katıldın.";
                }
            }
        }

    }
}
