namespace BO
{
    [Serializable]
    public abstract class BOException : Exception
    {
     
        protected BOException(string? message) : base(message) { }
        protected BOException(string message, Exception innerException)
            : base(message, innerException) { }

       
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

