namespace Evant.Interfaces
{
    public interface ILogHelper
    {
        void Log(string table, string status, string message = null, string ex = null);
    }
}
