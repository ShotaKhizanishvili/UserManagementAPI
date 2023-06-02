namespace UserManagementAPI.BLL.JsonModels
{
    public class UserTodos
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }
    }
}
