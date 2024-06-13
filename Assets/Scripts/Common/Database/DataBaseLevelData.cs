using SQLite4Unity3d;

namespace Common.Database
{
    public class DataBaseLevelData
    {
        [PrimaryKey] 
        public int id { get; set; }
        public int counter { get; set; }
    }
}