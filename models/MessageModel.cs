namespace oodb_project.models
{
    /// <summary>
    /// Модель сообщений
    /// </summary>
    public class MessageModel
    {
        public MessageModel()
        {
        }

        public MessageModel(string? message)
        {
            this.message = message;
        }

        public string? message { get; set; }
    }
}
