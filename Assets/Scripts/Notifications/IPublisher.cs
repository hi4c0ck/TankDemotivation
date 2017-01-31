using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Notifications
{
    public interface IPublisher
    {
        void SetUpSubscriber(ISubscriber subscriber);
        void SendMessage(INotification notification);
    }
}
