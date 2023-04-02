namespace Pomodoro.Core.Exceptions
{

    [Serializable]
    public class PomoMailException : PomoException
    {
        public PomoMailException() { }
        public PomoMailException(string message) : base(message) { }
        public PomoMailException(string message, Exception inner) : base(message, inner) { }
        protected PomoMailException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
