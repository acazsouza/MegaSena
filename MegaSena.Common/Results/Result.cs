namespace MegaSena.Results
{
    public abstract class Result
    {
        public bool Success { get; set; }
        public string Content { get; set; }
        public string Error { get; set; }
    }
}