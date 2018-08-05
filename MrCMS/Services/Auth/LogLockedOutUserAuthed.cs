﻿using System.Web;
using Microsoft.AspNetCore.Http;
using MrCMS.Entities.People;
using MrCMS.Helpers;
using MrCMS.Settings;
using MrCMS.Website;
using ISession = NHibernate.ISession;

namespace MrCMS.Services.Auth
{
    public class LogLockedOutUserAuthed : IOnLockedOutUserAuthed
    {
        private readonly SecuritySettings _securitySettings;
        private readonly ISession _session;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogLockedOutUserAuthed(SecuritySettings securitySettings, ISession session, IHttpContextAccessor httpContextAccessor)
        {
            _securitySettings = securitySettings;
            _session = session;
            _httpContextAccessor = httpContextAccessor;
        }
        public void Execute(UserLockedOutEventArgs args)
        {
            if (!_securitySettings.LogLoginAttempts)
                return;
            var request = _httpContextAccessor.HttpContext.Request;
            var loginAttempt = new LoginAttempt
            {
                User = args.User,
                Email = args.User?.Email,
                Status = LoginAttemptStatus.LockedOut,
                IpAddress = request.GetCurrentIP(),
                UserAgent = request.UserAgent()
            };
            _session.Transact(session => session.Save(loginAttempt));
        }
    }
}