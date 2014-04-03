using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

    public class CommentDAL : DALBase
    {
        #region CRUD-metoder


        /// <summary>
        /// Gets a comment
        /// </summary>
        /// <param name="memberId">The post id that the comments is made on</param>
        /// <returns>List of comments.</returns>
        public List<Comment> GetCommentsByPostId(int postId)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                try
                {
                    var comments = new List<Comment>(100);

                    MySqlCommand cmd = new MySqlCommand("GetCommentsByPostId", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", postId);

                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int memberIdIndex = reader.GetOrdinal("MemberId");
                            int postIdIndex = reader.GetOrdinal("PostId");
                            int commentIdIndex = reader.GetOrdinal("CommentId");
                            int valueIndex = reader.GetOrdinal("Value");

                            comments.Add(new Comment
                            {
                                MemberId = reader.GetInt32(memberIdIndex),  
                                PostId = reader.GetInt32(postIdIndex),
                                CommentId = reader.GetInt32(commentIdIndex),
                                Value = reader.GetString(valueIndex)
                            });
                        }
                    }

                    comments.TrimExcess();

                    return comments;
                }
                catch
                {
                    throw new ApplicationException(GenericErrorMessage);
                }
            }
        }

        /// <summary>
        /// Creates a comment and adds it to the table
        /// </summary>
        /// <param name="member">Comment to insert</param>
        public void InsertComment(Comment comment)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("InsertComment", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    MySqlCommand cmd2 = new MySqlCommand("GetCommentId", conn);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@MemberId", MySqlDbType.Int32, 4).Value = comment.MemberId;
                    cmd.Parameters.Add("@PostId", MySqlDbType.Int32, 4).Value = comment.PostId;
                    cmd.Parameters.Add("@Value", MySqlDbType.VarChar, 500).Value = comment.Value;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    using (var reader = cmd2.ExecuteReader())
                    {

                        reader.Read();
                        {
                            comment.CommentId = reader.GetInt32(0);
                        }
                    }
                }
                catch
                {
                    throw new ApplicationException(GenericErrorMessage);
                }
            }
        }

        /// <summary>
        /// Updates a comment
        /// </summary>
        /// <param name="member">Comment to update</param>
        public void UpdateComment(Comment comment)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("UpdateComment", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Id", MySqlDbType.Int32, 4).Value = comment.CommentId;
                    cmd.Parameters.Add("@PostId", MySqlDbType.Int32, 4).Value = comment.PostId;
                    cmd.Parameters.Add("@MemberId", MySqlDbType.Int32, 4).Value = comment.MemberId;
                    cmd.Parameters.Add("@newComment", MySqlDbType.VarChar, 500).Value = comment.Value;

                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw new ApplicationException(GenericErrorMessage);
                }
            }
        }

        /// <summary>
        /// Deletes comment
        /// </summary>
        /// <param name="memberId">Comments id</param>
        public void DeleteComment(int commentId)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("DeleteComment", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Id", MySqlDbType.Int32, 4).Value = commentId;

                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw new ApplicationException(GenericErrorMessage);
                }
            }
        }

        #endregion
    }
