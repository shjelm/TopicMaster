using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.ComponentModel;
using System.Web.Caching;

    /// <summary>
    /// Class to call handling of data
    /// </summary>
    [DataObject(true)]
    public class Service
    {    
        #region Fält

        private PostDAL _postDAL;
        private CommentDAL _commentDAL;
        private MemberDAL _memberDAL;

        #endregion

        #region Egenskaper

        private PostDAL PostDAL
        {
            get { return _postDAL ?? (_postDAL = new PostDAL()); }
        }


        private MemberDAL MemberDAL
        {
            get { return _memberDAL ?? (_memberDAL = new MemberDAL()); }
        }

        private CommentDAL CommentDAL
        {
            get { return _commentDAL ?? (_commentDAL = new CommentDAL()); }
        }

        #endregion

        #region Post CRUD-metoder

        /// <summary>
        /// Removes post.
        /// </summary>
        /// <param name="post">K</param>
        public void DeletePost(int postId) 
        {                                             
            PostDAL.DeletePost(postId);
        }

        /// <summary>
        /// Gets a specific post.
        /// </summary>
        /// <param name="postId">PostId</param>
        /// <returns>Post object</returnsP>.</returns>
        public Post GetPostByPostId(int postId)
        {
            return PostDAL.GetPostByPostId(postId);
        }

        /// <summary>
        /// Gets all posts.
        /// </summary>
        /// <returns>List of post objects</returns>
        public List<Post> GetPosts()
        {
            return PostDAL.GetPosts();
        }

        /// <summary>
        /// Save a post
        /// </summary>
        /// <param name="post">Post that will be saved</param>
        public void SavePost(Post post)
        {
            if (post.IsValid)
            {
                if (post.PostId == 0) //If PostId = 0 -> New post
                {
                    PostDAL.InsertPost(post);
                }
                else
                {
                    PostDAL.UpdatePost(post);
                }
            }
            else
            {
                ApplicationException ex = new ApplicationException(post.Error);
                ex.Data.Add("Post", post);
                throw ex;
            }
        }

        #endregion

        
        #region Member CRUD

        /// <summary>
        /// Gets alls members.
        /// </summary>
        /// <returns>List of member objects</returns>
        public List<Member> GetMembers()
        {
            return MemberDAL.GetMembers();
        }
        #endregion


        #region Comment CRUD-metoder

        /// <summary>
        /// Removes comment.
        /// </summary>
        /// <param name="comment">Comment to remove</param>
        public void DeleteComment(Comment comment) 
        {                                             
            CommentDAL.DeleteComment(comment.CommentId);
        }

        /// <summary>
        /// Get comments for specific post
        /// </summary>
        /// <param name="commentId">Post id</param>
        /// <returns>Comment object</returns>
        public List<Comment> GetCommentsByPostId(int postId)
        {
            return CommentDAL.GetCommentsByPostId(postId);
        }

        /// <summary>
        /// Save comment
        /// </summary>
        /// <param name="member">Comment to save</param>
        public void SaveComment(Comment comment)
        {
            if (comment.IsValid)
            {
                if (comment.CommentId == 0) 
                {
                    CommentDAL.InsertComment(comment);
                }
                else
                {
                    CommentDAL.UpdateComment(comment);
                }
            }
            else
            {
                ApplicationException ex = new ApplicationException(comment.Error);
                ex.Data.Add("Comment", comment);
                throw ex;
            }
        }
        #endregion
    }