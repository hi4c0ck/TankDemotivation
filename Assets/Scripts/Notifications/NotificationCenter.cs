using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Notifications
{
    public class NotificationCenter:MonoBehaviour,IPublisher,ISubscriber
    {
        
        List<ISubscriber> subscribers=new List<ISubscriber>();

        public void AddSubscriber(ISubscriber sub)
        {
            subscribers.Add(sub);
        }
        
        public void OnNotify(INotification notification)
        {
            SendMessage(notification);
        }

        public void SendMessage(INotification notification)
        {
            for (int i = 0; i < subscribers.Count; i++)
            {
                subscribers[i].OnNotify(notification);
            }
        }

        public void SetUpSubscriber(ISubscriber subscriber)
        {
            AddSubscriber(subscriber);
        }
    }
}
