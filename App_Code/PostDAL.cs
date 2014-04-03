using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

    public class PostDAL : DALBase
    {
        #region CRUD-metoder

        /// <summary>
        /// Gets all posts 
        /// </summary>
        /// <returns>List of posts</returns>
        public List<Post> GetPosts()
        {
            using (var conn = CreateConnection())
            {
                try
                {
                    var posts = new List<Post>(100);

                    var cmd = new MySqlCommand("GetPosts", conn); 
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        var memberIdIndex = reader.GetOrdinal("MemberId");
                        var valueIndex = reader.GetOrdinal("Value");
                        int postIdIndex = reader.GetOrdinal("PostId");

                        while (reader.Read())
                        {
                            posts.Add(new Post
                            {
                                MemberId = reader.GetInt32(memberIdIndex),
                                Value = reader.GetString(valueIndex),
                                PostId = reader.GetInt32(postIdIndex)
                            });
                        }
                    }

                    posts.TrimExcess();

                    return posts;
                }
                catch
                {
                    throw new ApplicationException(GenericErrorMessage);
                }
            }
        }

        /// <summary>
        /// Gets a specific post
        /// </summary>
        /// <param name="postId">Posts id </param>
        /// <returns>Post object</returns>
        public Post GetPostByPostId(int postId)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("GetPost", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", postId);

                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int memberIdIndex = reader.GetOrdinal("MemberId");
                            int postIdIndex = reader.GetOrdinal("PostId");
                            int valueIndex = reader.GetOrdinal("Value");

                            return new Post
                            {
                                MemberId = reader.GetInt32(memberIdIndex),
                                PostId = reader.GetInt32(postIdIndex),
                                Value = reader.GetString(valueIndex)
                            };
                        }
                    }
                    return null;
                }
                catch
                {
                    throw new ApplicationException(GenericErrorMessage);
                }
            }
        }

        /// <summary>
        /// Inserts post
        /// </summary>
        /// <param name="post">Post to insert</param>
        public void InsertPost(Post post)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("InsertPost", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    MySqlCommand cmd2 = new MySqlCommand("GetPostId", conn);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Id", MySqlDbType.Int32, 4).Value = post.MemberId;
                    cmd.Parameters.Add("@Value", MySqlDbType.VarChar, 500).Value = post.Value;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    using (var reader = cmd2.ExecuteReader())
                    {

                        reader.Read();
                        {
                            post.PostId = reader.GetInt32(0);
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
        /// Update post
        /// </summary>
        /// <param name="post">Post to update.</param>
        public void UpdatePost(Post post)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("UpdatePost", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Id", MySqlDbType.Int32, 4).Value = post.PostId;
                    cmd.Parameters.Add("@MemberId", MySqlDbType.Int32, 4).Value = post.MemberId;
                    cmd.Parameters.Add("@Value", MySqlDbType.VarChar, 500).Value = post.Value;

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
        /// Deletes post
        /// </summary>
        /// <param name="postId">Posts id.</param>
        public void DeletePost(int postId)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("DeletePost", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Id", MySqlDbType.Int32, 4).Value = postId;

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