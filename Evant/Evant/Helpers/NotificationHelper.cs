using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.DAL.Repositories.Interfaces;
using Evant.Interfaces;
using Evant.NotificationCenter.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Evant.Constants.NotificationConstant;

namespace Evant.Helpers
{
    public class NotificationHelper : INotificationHelper
    {
        private readonly IUserRepository _userRepo;
        private readonly IRepository<UserDevice> _userDeviceRepo;
        private readonly IRepository<UserSetting> _userSettingRepo;
        private readonly IOneSignal _oneSignal;


        public NotificationHelper(IUserRepository userRepo,
            IRepository<UserDevice> userDeviceRepo,
            IRepository<UserSetting> userSettingRepo,
            IOneSignal oneSignal)
        {
            _userRepo = userRepo;
            _userDeviceRepo = userDeviceRepo;
            _userSettingRepo = userSettingRepo;
            _oneSignal = oneSignal;
        }


        public async Task SendFollowNotification(Guid senderId, Guid receiverId)
        {
            var playerIds = (await _userDeviceRepo.Where(d => d.UserId == receiverId && d.IsLoggedin))
                .Select(t => t.DeviceId)
                .ToList();

            if (!playerIds.IsNullOrEmpty())
            {
                var senderUser = await _userRepo.First(u => u.Id == senderId);
                if (senderUser != null)
                {
                    var receiverUserSetting = await _userSettingRepo.First(s => s.UserId == receiverId);
                    if (receiverUserSetting != null && receiverUserSetting.IsFriendshipNotif)
                    {
                        string content = string.Format("{0} {1} seni takip etmeye başladı", senderUser.FirstName, senderUser.LastName);
                        var result = _oneSignal.SendNotification(playerIds, content);
                    }
                }
            }
        }

        public async Task SendEventAttendNotification(Guid senderId, Guid receiverId)
        {
            var playerIds = (await _userDeviceRepo.Where(d => d.UserId == receiverId && d.IsLoggedin))
                .Select(t => t.DeviceId)
                .ToList();

            if (!playerIds.IsNullOrEmpty())
            {
                var senderUser = await _userRepo.First(u => u.Id == senderId);
                if (senderUser != null)
                {
                    var receiverUserSetting = await _userSettingRepo.First(s => s.UserId == receiverId);
                    if (receiverUserSetting != null && receiverUserSetting.IsEventNewComerNotif)
                    {
                        string content = string.Format("{0} {1}, etkinliğinize katıldı.", senderUser.FirstName, senderUser.LastName);
                        var result = _oneSignal.SendNotification(playerIds, content);
                    }
                }
            }
        }

        public async Task SendCommentNotification(Guid senderId, Guid receiverId)
        {
            var playerIds = (await _userDeviceRepo.Where(d => d.UserId == receiverId && d.IsLoggedin))
                .Select(t => t.DeviceId)
                .ToList();

            if (!playerIds.IsNullOrEmpty())
            {
                var senderUser = await _userRepo.First(u => u.Id == senderId);
                if (senderUser != null)
                {
                    var receiverUserSetting = await _userSettingRepo.First(s => s.UserId == receiverId);
                    if (receiverUserSetting != null && receiverUserSetting.IsCommentNotif)
                    {
                        string content = string.Format("{0} {1}, bir etkinliğinize yorum yaptı.", senderUser.FirstName, senderUser.LastName);
                        var result = _oneSignal.SendNotification(playerIds, content);
                    }
                }
            }
        }

        public async Task SendEventUpdateNotification(Guid receiverId)
        {
            var playerIds = (await _userDeviceRepo.Where(d => d.UserId == receiverId && d.IsLoggedin))
                .Select(t => t.DeviceId)
                .ToList();

            if (!playerIds.IsNullOrEmpty())
            {
                var receiverUserSetting = await _userSettingRepo.First(s => s.UserId == receiverId);
                if (receiverUserSetting != null && receiverUserSetting.IsEventUpdateNotif)
                {
                    string content = string.Format("Katıldığız bir etkinlik güncellendi.");
                    var result = _oneSignal.SendNotification(playerIds, content);
                }
            }
        }

    }
}
