namespace MonopolyGo
{
    public interface ISaveStorage
    {
        void Save(string json);
        string Load();
        bool HasData();
    }
}
