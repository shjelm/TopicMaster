using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for LogDAL
/// </summary>
public class LogDAL :DALBase
{
        public void WriteToLog(string msg, int userid)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("WriteToLog", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserId", MySqlDbType.Int32, 4).Value = userid;
                    cmd.Parameters.Add("@Message", MySqlDbType.VarChar, 500).Value = msg;

                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw new ApplicationException(GenericErrorMessage);
                }
            }
        }
}