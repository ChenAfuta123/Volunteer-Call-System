namespace BO
{
    /// <summary>
    /// Base class for custom exceptions in the Business Object (BO) layer.
    /// </summary>
    [Serializable]
    public abstract class BOException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the BOException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected BOException(string? message) : base(message) { }
     
        /// <summary>
        /// Initializes a new instance of the BOException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The exception that caused the current exception.</param>
        protected BOException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Returns a string representation of the exception, including its type, message, and inner exception details (if any).
        /// </summary>
        /// <returns>A string representation of the exception.</returns>
        public override string ToString()
        {
            string result = $"Exception Type: {GetType().Name}\nMessage: {Message}";

           
            if (InnerException != null)
            {
                result += $"\nInner Exception: {InnerException.GetType().Name}\nInner Message: {InnerException.Message}";
            }

            return result;
        }
    }
}

