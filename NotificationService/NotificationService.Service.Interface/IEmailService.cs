﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Service.Interface
{
    public interface IEmailService
    {
        void SendEmail(Model.Notification notification);
    }
}