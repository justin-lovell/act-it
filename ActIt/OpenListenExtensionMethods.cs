using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ActIt
{
    public static class OpenListenExtensionMethods
    {
        private static readonly MethodInfo OpenProcessEventToHandlerMethodInfo;

        static OpenListenExtensionMethods()
        {
            const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
            OpenProcessEventToHandlerMethodInfo =
                typeof (OpenListenExtensionMethods).GetMethod("ProcessEventToHandlerGeneric", bindingFlags);
        }

        private static Task ProcessEventToHandlerGeneric<THandler, TEvent>(
            THandler theHandler,
            TEvent theEvent,
            SceneActor actor)
            where THandler : OpenPlotListenerHandler<TEvent>
        {
            return theHandler.Handle(theEvent, actor);
        }

        private static Task ProcessEventToHandler(Type concreteHandlerType, object theEvent, SceneActor actor)
        {
            var typeArguments = new[] {concreteHandlerType, theEvent.GetType()};
            var genericMethodInfo = OpenProcessEventToHandlerMethodInfo.MakeGenericMethod(typeArguments);

            var concreteHandlerInstance = Activator.CreateInstance(concreteHandlerType);
            object[] parameters = {concreteHandlerInstance, theEvent, actor};

            return (Task) genericMethodInfo.Invoke(null, parameters);
        }

        public static void OpenlyListen(this PlotBuilder plotBuilder, Type handlerType)
        {
            if (handlerType == null)
            {
                throw new ArgumentNullException("handlerType");
            }

            if (handlerType.BaseType == null || !handlerType.BaseType.IsGenericType
                || handlerType.BaseType.GetGenericTypeDefinition() != typeof (OpenPlotListenerHandler<>))
            {
                // todo: handlerType implements OpenPlotListenerHandler
                throw new NotImplementedException();
            }

            // todo: type could be newed up

            // todo: verify that the open generic parameters are same length
            // todo: ensure that type constraints are obeyed (handler type)

            var listenForGenericType = handlerType.BaseType.GetGenericArguments()[0].GetGenericTypeDefinition();

            plotBuilder.EavesDrop((@event, actor) =>
            {
                var theEventType = @event.GetType();
                var isInterestingEvent = theEventType.IsGenericType
                                         && theEventType.GetGenericTypeDefinition() == listenForGenericType;

                if (!isInterestingEvent)
                {
                    return TaskEx.IntoTaskResult<object>(null);
                }

                // todo: 
                var concreteType = handlerType.MakeGenericType(theEventType.GetGenericArguments());
                return ProcessEventToHandler(concreteType, @event, actor);
            });
        }
    }
}
