// <copyright file="PomoMailException.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Exceptions;

namespace Pomodoro.Services.Exceptions
{
    /// <summary>
    /// Throws when model constraints violated.
    /// </summary>
    [Serializable]
    public class PomoMailException : PomoException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PomoMailException"/> class.
        /// </summary>
        public PomoMailException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PomoMailException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public PomoMailException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PomoMailException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public PomoMailException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PomoMailException"/> class.
        /// </summary>
        /// <param name="info">Stores all the data needed to serialize or deserialize an object.</param>
        /// <param name="context">Source and destination of serialization context.</param>
        protected PomoMailException(
                System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
