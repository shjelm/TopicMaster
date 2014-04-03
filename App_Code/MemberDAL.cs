using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel;
using System.Web.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;

    /// <summary>
    /// Class MemberDAL.
    /// </summary>
    [DataObject(false)]
    public class MemberDAL : DALBase
    {
        #region CRUD-metoder

        /// <summary>
        /// Get all members
        /// </summary>
        /// <returns>List of members</returns>
        public List<Member> GetMembers()
        {
            using (var conn = CreateConnection())
            {
                try
                {
                    var members = new List<Member>(100);

                    var cmd = new MySqlCommand("GetMembers", conn); 
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        var memberIdIndex = reader.GetOrdinal("id");
                        var nameIndex = reader.GetOrdinal("name");

                        while (reader.Read())
                        {
                            members.Add(new Member
                            {
                                userId = reader.GetInt32(memberIdIndex),
                                Username = reader.GetString(nameIndex)
                            });
                        }
                    }

                    members.TrimExcess();

                    return members;
                }
                catch
                {
                    throw new ApplicationException(GenericErrorMessage);
                }
            }
        }
        #endregion
    }