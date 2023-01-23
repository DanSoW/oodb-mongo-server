using oodb_mongo_server.models.data;

namespace oodb_project.models
{
    /// <summary>
    /// Модель, характеризующая администратора приложения для мониторинга
    /// </summary>
    public class AdminDataModel : IdDataModel
    {
        public AdminDataModel() : base()
        {
        }

        public AdminDataModel(string id, string email) : base(id)
        {
            Email = email;
        }

        public string Email { get; set; }
    }
}
