using MySql.Data;
using MySql.Data.MySqlClient;
namespace StoreOrderTracker.Server.Data;

public class DbConnection
{
    public string Server { get; set; }
    public string DatabaseName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public MySqlConnection Connection { get; set;}

    private static DbConnection _instance = null;
    public static DbConnection Instance()
    {
        if (_instance == null)
        {
            _instance = new DbConnection();
        }
        return _instance;
    }
    
    public bool IsConnect()
    {
        if (Connection == null)
        {
            if (String.IsNullOrEmpty(DatabaseName))
            {
                return false;
            }
                
            var connectionString = $"Server={Server}; database={DatabaseName}; UID={UserName}; password={Password}";
            Connection = new MySqlConnection(connectionString);
            Connection.Open();
        }
    
        return true;
    }
    
    public void Close()
    {
        Connection.Close();
    }        
}