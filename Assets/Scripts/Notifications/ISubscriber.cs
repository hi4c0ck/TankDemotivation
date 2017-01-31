using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Notifications
{
    public interface ISubscriber
    {
        void OnNotify(INotification notification);
    }
}
