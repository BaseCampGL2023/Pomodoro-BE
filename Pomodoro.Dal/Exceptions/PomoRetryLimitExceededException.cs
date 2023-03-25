// <copyright file="PomoRetryLimitExceededException.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Dal.Exceptions
{
    /// <summary>
    /// Wrapped and logged RetryLimitExceededException.
    /// </summary>
    [Serializable]
    public class PomoRetryLimitExceededException : PomoException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PomoRetryLimitExceededException"/> class.
        /// </summary>
        public PomoRetryLimitExceededException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PomoRetryLimitExceededException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public PomoRetryLimitExceededException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PomoRetryLimitExceededException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public PomoRetryLimitExceededException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PomoRetryLimitExceededException"/> class.
        /// </summary>
        /// <param name="info">Stores all the data needed to serialize or deserialize an object.</param>
        /// <param name="context">Source and destination of serialization context.</param>
        protected PomoRetryLimitExceededException(
                System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
