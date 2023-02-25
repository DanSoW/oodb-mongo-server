/*
 * В данном файле располагаются все константы, которые используются
 * для маршрутизации в рамках данного сервиса.
 * **/

namespace oodb_project.constants
{
    public class ApiUrl
    {
        /* Запросы на сохранение данных */
        static public string API_SAVE_HOST              = "/api/host/save";

        static public string API_SAVE_ADMIN             = "/api/admin/save";

        static public string API_SAVE_DATA_SOURCE       = "/api/data-source/save";

        static public string API_SAVE_SERVICE           = "/api/service/save";

        static public string API_SAVE_HOST_SERVICE      = "/api/host-service/save";

        static public string API_SAVE_MONITOR_APP       = "/api/monitor-app/save";

        /* Запросы на получение данных */
        static public string API_GET_HOST               = "/api/host/get/{id}";
        static public string API_GET_ALL_HOST           = "/api/host/get/all";
        static public string API_COMPLEX_HOST           = "/api/host/get/complex";
        static public string API_COMPLEX_LINQ_HOST      = "/api/host/get/complex-linq";

        static public string API_GET_ADMIN              = "/api/admin/get/{id}";
        static public string API_GET_ALL_ADMIN          = "/api/admin/get/all";

        static public string API_GET_DATA_SOURCE        = "/api/data-source/get/{id}";
        static public string API_GET_ALL_DATA_SOURCE    = "/api/data-source/get/all";

        static public string API_GET_SERVICE            = "/api/service/get/{id}";
        static public string API_GET_ALL_SERVICE        = "/api/service/get/all";
        static public string API_COMPLEX_SERVICE        = "/api/service/get/all/complex";
        static public string API_COMPLEX_LINQ_SERVICE   = "/api/service/get/all/complex-linq";

        static public string API_GET_HOST_SERVICE       = "/api/host-service/get/{id}";
        static public string API_GET_ALL_HOST_SERVICE   = "/api/host-service/get/all";

        static public string API_GET_MONITOR_APP        = "/api/monitor-app/get/{id}";
        static public string API_GET_ALL_MONITOR_APP    = "/api/monitor-app/get/all";
        static public string API_GET_SODA_MONITOR_APP   = "/api/monitor-app/get/soda";

        /* Запрос на изменение данных */
        static public string API_UPDATE_HOST            = "/api/host/update";
        static public string API_UPDATE_ADMIN           = "/api/admin/update";
        static public string API_UPDATE_DATA_SOURCE     = "/api/data-source/update";
        static public string API_UPDATE_SERVICE         = "/api/service/update";
        static public string API_UPDATE_HOST_SERVICE    = "/api/host-service/update";
        static public string API_UPDATE_MONITOR_APP     = "/api/monitor-app/update";

        /* Запрос на удаление */
        static public string API_DELETE_HOST            = "/api/host/delete/{id}";
        static public string API_DELETE_ADMIN           = "/api/admin/delete/{id}";
        static public string API_DELETE_DATA_SOURCE     = "/api/data-source/delete/{id}";
        static public string API_DELETE_SERVICE         = "/api/service/delete/{id}";
        static public string API_DELETE_HOST_SERVICE    = "/api/host-service/delete/{id}";
        static public string API_DELETE_MONITOR_APP     = "/api/monitor-app/delete/{id}";
    }
}
