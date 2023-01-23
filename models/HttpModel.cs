namespace oodb_project.models
{
    public class HttpModel
    {
        public HttpModel()
        {
        }

        public HttpModel(string? path, string? payload)
        {
            Path = path;
            Payload = payload;
        }

        public string? Path { get; set; }
        public string? Payload { get; set; }
    }
}
