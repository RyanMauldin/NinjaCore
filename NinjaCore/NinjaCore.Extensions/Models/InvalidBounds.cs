namespace NinjaCore.Extensions.Models
{
    public class InvalidBounds
    {
        public readonly string ArgumentName;

        public readonly string ErrorMessage;

        public InvalidBounds(string argumentName, string errorMessage)
        {
            ArgumentName = argumentName;
            ErrorMessage = errorMessage;
        }
    }
}
