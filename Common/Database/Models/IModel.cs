namespace Common.Database.Models
{
    public interface IModel
    {
        public abstract void Save();
        public abstract void Delete();
    }
}
