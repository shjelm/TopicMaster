using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Caching;

    /// <summary>
    /// Klassen tillhandahåller metoder presentationslogiklagret
    /// anropar för att hantera data. Främst innehåller klassen
    /// metoder som använder sig av klasser i dataåtkomstlagret.
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
            // Ett PostDAL-objekt skapas först då det behövs för första 
            // gången (lazy initialization, http://en.wikipedia.org/wiki/Lazy_initialization).
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
        // http://en.wikipedia.org/wiki/Create,_read,_update_and_delete

        /// <summary>
        /// Tar bort specifierad kontaktuppgift ur databasen.
        /// </summary>
        /// <param name="post">Kontaktuppgift som ska tas bort.</param>
        public void DeletePost(int postId) // ObjectDataSource kräver att en Delete-metod
        {                                             // med en parameter av typen Member.
            PostDAL.DeletePost(postId);
        }

        /// <summary>
        /// Hämtar kontaktuppgift med ett specifikt nummer från databasen.
        /// </summary>
        /// <param name="postId">Kontaktuppgiftens nummer.</param>
        /// <returns>Ett Post-objekt innehållande kontaktuppgifter.</returns>
        public Post GetPostByPostId(int postId)
        {
            return PostDAL.GetPostByPostId(postId);
        }

        /// <summary>
        /// Hämtar en kunds kontaktuppgifter som finns lagrade i databasen.
        /// </summary>
        /// <returns>Lista med referenser till Post-objekt innehållande kontaktuppgifter.</returns>
        public List<Post> GetPosts()
        {
            return PostDAL.GetPosts();
        }

        /// <summary>
        /// Spara en kontaktuppgift i databasen.
        /// </summary>
        /// <param name="member">KOntaktuppgifter som ska sparas.</param>
        public void SavePost(Post post)
        {
            if (post.IsValid)
            {
                // Post-objektet sparas antingen genom att en ny post 
                // skapas eller genom att en befintlig post uppdateras.
                if (post.PostId == 0) // Ny post om PostID är 0!
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
                // Uppfyller inte objektet affärsreglerna kastas ett undantag med
                // ett allmänt felmeddelande samt en referens till objektet som 
                // inte klarade valideringen.
                ApplicationException ex = new ApplicationException(post.Error);
                ex.Data.Add("Post", post);
                throw ex;
            }
        }

        #endregion

        
        #region Member CRUD-metoder

        /// <summary>
        /// Tar bort specifierad kund ur databasen.
        /// </summary>
        /// <param name="memberId">Kund med kundnummer som ska tas bort.</param>
        public void DeleteMember(int memberId)
        {
            MemberDAL.DeleteMember(memberId);
        }

        /// <summary>
        /// Hämtar en kund med ett specifikt kundnummer från databasen.
        /// </summary>
        /// <param name="memberId">Kundens kundnummer.</param>
        /// <returns>Ett Member-objekt innehållande kunduppgifter.</returns>
        public Member GetMember(int memberId)
        {
            return MemberDAL.GetMemberById(memberId);
        }

        /// <summary>
        /// Hämtar alla kunder som finns lagrade i databasen.
        /// </summary>
        /// <returns>Lista med referenser till Member-objekt innehållande kunduppgifter.</returns>
        public List<Member> GetMembers()
        {
            return MemberDAL.GetMembers();
        }

        /// <summary>
        /// Spara en kunds kunduppgifter i databasen.
        /// </summary>
        /// <param name="member">Kunduppgifter som ska sparas.</param>
        //public void SaveMember(Member member)
        //{
        //    // Klarar objektet validering i affärslogiklagret?
        //    if (member.IsValid)
        //    {
        //        // Member-objektet sparas antingen genom att en ny post 
        //        // skapas eller genom att en befintlig post uppdateras.
        //        if (member.MemberId == 0) // Ny post om MemberId är 0!
        //        {
        //            MemberDAL.InsertMember(member);
        //        }
        //        else
        //        {
        //            MemberDAL.UpdateMember(member);
        //        }
        //    }
        //    else
        //    {
        //        // Uppfyller inte objektet affärsreglerna kastas ett undantag med
        //        // ett allmänt felmeddelande samt en referens till objektet som 
        //        // inte klarade valideringen.
        //        ApplicationException ex = new ApplicationException(member.Error);
        //        ex.Data.Add("Member", member);
        //        throw ex;
        //    }
        //}

        #endregion


        #region Comment CRUD-metoder

        /// <summary>
        /// Tar bort specifierad comment ur databasen.
        /// </summary>
        /// <param name="comment">Kontaktuppgift som ska tas bort.</param>
        public void DeleteComment(Comment comment) // ObjectDataSource kräver att en Delete-metod
        {                                             // med en parameter av typen Member.
            CommentDAL.DeleteComment(comment.CommentId);
        }

        /// <summary>
        /// Hämtar kontaktuppgift med ett specifikt nummer från databasen.
        /// </summary>
        /// <param name="commentId">Kontaktuppgiftens nummer.</param>
        /// <returns>Ett Post-objekt innehållande kontaktuppgifter.</returns>
        public Comment GetCommentByCommentId(int commentId)
        {
            return CommentDAL.GetCommentByCommentId(commentId);
        }

        /// <summary>
        /// Spara en kontaktuppgift i databasen.
        /// </summary>
        /// <param name="member">KOntaktuppgifter som ska sparas.</param>
        public void SaveComment(Comment comment)
        {
            if (comment.IsValid)
            {
                // Post-objektet sparas antingen genom att en ny post 
                // skapas eller genom att en befintlig post uppdateras.
                if (comment.CommentId == 0) // Ny post om PostID är 0!
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
                // Uppfyller inte objektet affärsreglerna kastas ett undantag med
                // ett allmänt felmeddelande samt en referens till objektet som 
                // inte klarade valideringen.
                ApplicationException ex = new ApplicationException(comment.Error);
                ex.Data.Add("Comment", comment);
                throw ex;
            }
        }


        #endregion
    }