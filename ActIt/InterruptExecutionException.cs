using System;
using System.Runtime.Serialization;

namespace ActIt
{
    [Serializable]
    public class InterruptExecutionException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public InterruptExecutionException()
        {
        }

        public InterruptExecutionException(string message) : base(message)
        {
        }

        public InterruptExecutionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InterruptExecutionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}