

namespace SmartInventory.Model
{
    public interface IEntity<T1>
    {
        T1 id { get; set; }
        T1 CreatedBy { get; set; }
        DateTime CreatedTime { get; set; }

        T1 UpdatedBy { get; set; }


        DateTime UpdatedTime { get; set; }
    }

    public class Entity : IEntity<int>
    {
        public int id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }

        public int UpdatedBy { get; set; }


        public DateTime UpdatedTime { get; set; }
    }

    public class Entity2: IEntity<string>{
        
        public string id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedTime { get; set; }

    public string UpdatedBy { get; set; }


    public DateTime UpdatedTime { get; set; }
}

}