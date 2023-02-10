namespace Pomodoro.Core.Exceptions
{

    [Serializable]
    public class PomoException : Exception
    {
        public PomoException() { }
        public PomoException(string message) : base(message) { }
        public PomoException(string message, Exception inner) : base(message, inner) { }
        protected PomoException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
