using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using DbConnection = StoreOrderTracker.Server.Data.DbConnection;

namespace StoreOrderTracker.Server.Models.Services;

public class CustomerOrderRepository
{
    private readonly DatabaseSettings _settings;

    public CustomerOrderRepository(IOptions<DatabaseSettings> settings)
    {
        _settings = settings.Value;
    }

    public void AddCustomerOrder(CustomerOrder customerOrder)
    {
        var dbCon = DbConnection.Instance();
        dbCon.Server = _settings.Server;
        dbCon.DatabaseName = _settings.DatabaseName;
        dbCon.UserName = _settings.DatabaseUser;
        dbCon.Password = _settings.Password;
        
        if (dbCon.IsConnect())
        {
            var customerId = "";
            const string customerIdQuery = "SELECT CustomerId FROM Customer where CustomerName =  @customer_name AND CustomerPhoneNumber = @customer_phone_number";
            var customerIdCmd = new MySqlCommand(customerIdQuery, dbCon.Connection);
            customerIdCmd.Parameters.AddWithValue("@customer_name", customerOrder.CustomerName);
            customerIdCmd.Parameters.AddWithValue("@customer_phone_number", customerOrder.CustomerPhoneNumber);
            var reader = customerIdCmd.ExecuteReader();
            while(reader.Read())
            {
                customerId = reader.GetUInt64("CustomerId").ToString();
            }
            reader.Close();

            if (customerId == "")
            {
                const string insertCustomerQuery = "INSERT INTO Customer (CustomerName, CustomerPhoneNumber) VALUES (@customer_name, @customer_phone_number)";
                var insertCustomerCmd = new MySqlCommand(insertCustomerQuery, dbCon.Connection);
                insertCustomerCmd.Parameters.AddWithValue("@customer_name", customerOrder.CustomerName);
                insertCustomerCmd.Parameters.AddWithValue("@customer_phone_number", customerOrder.CustomerPhoneNumber);
                insertCustomerCmd.ExecuteNonQuery();

                reader = customerIdCmd.ExecuteReader();
                while(reader.Read())
                {
                    customerId = reader.GetUInt64("CustomerId").ToString();
                }
                reader.Close();
            }

            const string insertCustomerOrderQuery = "INSERT INTO CustomerOrder (CustomerId, CustomerOrder, AmountDue, OrderDate) VALUES (@customer_id, @customer_Order, @amount_due, @order_date)";
            var insertCustomerOrderCmd = new MySqlCommand(insertCustomerOrderQuery, dbCon.Connection);
            insertCustomerOrderCmd.Parameters.AddWithValue("@customer_id", customerId);
            insertCustomerOrderCmd.Parameters.AddWithValue("@customer_Order", customerOrder.Order);
            insertCustomerOrderCmd.Parameters.AddWithValue("@amount_due", customerOrder.AmountDue);
            insertCustomerOrderCmd.Parameters.AddWithValue("@order_date", DateTime.Now);
            insertCustomerOrderCmd.ExecuteNonQuery();
            
            dbCon.Close();
        }
    }
}