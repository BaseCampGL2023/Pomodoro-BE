// <copyright file="PomoBrokenModelException.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Services.Exceptions
{
    /// <summary>
    /// Throws when model constraints violated.
    /// </summary>
    [Serializable]
    public class PomoBrokenModelException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PomoBrokenModelException"/> class.
        /// </summary>
        public PomoBrokenModelException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PomoBrokenModelException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public PomoBrokenModelException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PomoBrokenModelException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public PomoBrokenModelException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PomoBrokenModelException"/> class.
        /// </summary>
        /// <param name="info">Stores all the data needed to serialize or deserialize an object.</param>
        /// <param name="context">Source and destination of serialization context.</param>
        protected PomoBrokenModelException(
                System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
