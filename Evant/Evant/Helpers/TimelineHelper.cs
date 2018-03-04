using Evant.DAL.EF.Tables;
using System;
using System.Collections.Generic;

namespace Evant.Helpers
{
    public static class TimelineHelper
    {
        public static string GenerateMyCommentBody(Guid userId, Comment comment)
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

        public static string GenerateMyFollowingBody()
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

        public static string GenerateMyFollowerBody()
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

        public static string GenerateMyCreateEventBody(Event @event)
        {
            int index = new Random().Next(0, 100);

            //Category
            if (index % 3 == 0)
            {
                return @event.Category.Name + " kategorisinde bir etkinlik oluşturdun.";
            }
            else if (index % 3 == 1)
            {
                return @event.City + "'de yeni bir etkinlik oluşturdun.";
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

        public static string GenerateMyJoinEventBody(Event @event)
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


        public static string GenerateUserCommentBody(Guid userId, Comment comment)
        {
            if (userId == comment.Event.UserId)
            {
                return "Etkinliğine yorum bıraktı: " + comment.Content;
            }
            else
            {
                return "Yeni yorum bıraktı: " + comment.Content;
            }
        }

        public static string GenerateUserFollowingBody()
        {
            var contents = new List<string>()
            {
                "Çevresi genişlemeye başladı bile, yeni birini takip etti.",
                "Sosyalleşmeye başladı sanırım."
            };

            int index = new Random().Next(0, contents.Count);
            return contents[index];
        }

        public static string GenerateUserFollowerBody()
        {
            var contents = new List<string>()
            {
                "Bugün yine popüler, yeni bir takipçi kazandı!",
                "Takipçi sayısı bir tık arttı.",
                "Onu takip eden birisi var."
            };

            int index = new Random().Next(0, contents.Count);
            return contents[index];
        }

        public static string GenerateUserCreateEventBody(Event @event)
        {
            int index = new Random().Next(0, 100);

            //Category
            if (index % 3 == 0)
            {
                return @event.Category.Name + " kategorisinde bir etkinlik oluşturdu.";
            }
            else if (index % 3 == 1)
            {
                return @event.City + "'de yeni bir etkinlik oluşturdu.";
            }
            else
            {
                TimeSpan span = @event.FinishDate - @event.StartDate;
                if (span.Days == 0)
                {
                    return span.Hours + " saat sürecek bir etkinlik oluşturdu.";
                }
                else
                {
                    return span.Days + " gün" + span.Hours + " saat sürecek bir etkinlik oluşturdu.";
                }
            }
        }

        public static string GenerateUserJoinEventBody(Event @event)
        {
            int index = new Random().Next(0, 100);

            //Category
            if (index % 3 == 0)
            {
                return @event.Category.Name + " kategorisinde bir etkinliğe katıldı.";
            }
            else if (index % 3 == 1)
            {
                return @event.City + "'de yeni bir etkinliğe katıldı.";
            }
            else
            {
                TimeSpan span = @event.FinishDate - @event.StartDate;
                if (span.Days == 0)
                {
                    return span.Hours + " saat sürecek bir etkinliğe katıldı.";
                }
                else
                {
                    return span.Days + " gün" + span.Hours + " saat sürecek bir etkinliğe katıldı.";
                }
            }
        }

    }
}
