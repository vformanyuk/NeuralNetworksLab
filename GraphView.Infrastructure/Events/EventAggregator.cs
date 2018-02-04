using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NeuralNetworkLab.Infrastructure.Events
{
    public class EventAggregator
    {
        private static readonly ConcurrentDictionary<EventAggregatorSubscribtionToken, ActionContainer> _subscriptions;

        static EventAggregator()
        {
            _subscriptions = new ConcurrentDictionary<EventAggregatorSubscribtionToken, ActionContainer>();
        }

        public static EventAggregatorSubscribtionToken Subscribe<T>(Action<T> action) where T : EventAggregatorEventArgs
        {
            var token = new EventAggregatorSubscribtionToken();
            MethodInfo method = null;
#if (WINDOWS_PHONE_APP || WINDOWS_APP)  // targeting UWP
            method = action.GetMethodInfo();
#else
            method = action.Method;
#endif
            while (!_subscriptions.TryAdd(token, new ActionContainer(action.Target, method))) ;

            return token;
        }

        public static void Unsubscribe(EventAggregatorSubscribtionToken token)
        {
            if (!_subscriptions.ContainsKey(token)) return;

            while (!_subscriptions.TryRemove(token, out var _)) ;
        }

        public static void Publish<T>(T eventArgs) where T : EventAggregatorEventArgs
        {
            var containers = from c in _subscriptions.Values
                             let args = c.Action.GetParameters()
                             where args[0].ParameterType == typeof(T)
                             select c;

            foreach (var actionContainer in containers.ToList())
            {
                actionContainer.Action.Invoke(actionContainer.Target, new object[] { eventArgs });
            }
        }

        public static Task PublishAsync<T>(T eventArgs) where T : EventAggregatorEventArgs
        {
            var containers = from c in _subscriptions.Values
                             let args = c.Action.GetParameters()
                             where args[0].ParameterType == typeof(T)
                             select c;

            var whenAllTask = Task.WhenAll(containers.Select(c => Task.Run(() =>
            {
                c.Action.Invoke(c.Target, new object[] { eventArgs });
            })));

            return whenAllTask;
        }

        private class ActionContainer
        {
            public MethodInfo Action { get; }
            public Object Target { get; }

            public ActionContainer(Object target, MethodInfo actionInfo)
            {
                Target = target;
                Action = actionInfo;
            }
        }
    }
}
