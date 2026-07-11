namespace FileProcessor.API.Configurations
{
    public class APIKeyOptions
    {
        public string HeaderName { get; set; } = "x-api-key";
        public string Value { get; set; } = string.Empty;
    }
}
