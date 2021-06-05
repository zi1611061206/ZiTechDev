namespace ZiTechDev.CommonModel.Requests.User
{
    public class GenderItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public GenderItem(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
