using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Web.Configuration;

public abstract class DALBase
{
    #region Fält

    private static string _connectionString;

    protected static readonly string GenericErrorMessage = "An error occured in the DAL";

    #endregion

    #region Konstruktorer

    static DALBase()
    {
        _connectionString =
            WebConfigurationManager.ConnectionStrings["LocalMySqlServer"].ConnectionString;
    }

    #endregion

    #region Hjälpmetoder

    /// <summary>
    /// Creates connection
    /// </summary>
    /// <returns>Reference to MySQL connection</returns>
    protected MySqlConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }

    #endregion
}