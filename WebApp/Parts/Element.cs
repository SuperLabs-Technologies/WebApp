using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp
{
    internal class Element
    {
        public string Id { get; set; }

        public Element(string id)
        {
            this.Id = id;
            EventManager.Elements.Add(this);
        }

        public Dictionary<string, Action<EventArguments>> Events = new Dictionary<string, Action<EventArguments>>();
        public void AddEventListener(string eventName, Action<EventArguments> eventFunction)
        {
            Events.Add(eventName, eventFunction);
        }
    }
}
