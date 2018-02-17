namespace Evant.Interfaces
{
    public interface ILogHelper
    {
        void Log(string table, int statusCode, string ex = null, string message = null);
    }
}
