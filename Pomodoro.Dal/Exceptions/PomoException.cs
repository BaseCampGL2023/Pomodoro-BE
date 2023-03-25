// <copyright file="PomoException.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Dal.Exceptions
{
    /// <summary>
    /// Wrapped and logged system exception.
    /// </summary>
    [Serializable]
    public class PomoException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PomoException"/> class.
        /// </summary>
        public PomoException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PomoException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public PomoException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PomoException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public PomoException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PomoException"/> class.
        /// </summary>
        /// <param name="info">Stores all the data needed to serialize or deserialize an object.</param>
        /// <param name="context">Source and destination of serialization context.</param>
        protected PomoException(
                System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
