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
    /// Klassen MemberDAL.
    /// </summary>
    [DataObject(false)]
    public class MemberDAL : DALBase
    {
        #region CRUD-metoder

        /// <summary>
        /// Hämtar alla kunder i databasen.
        /// </summary>
        /// <returns>Lista med referenser till Member-objekt.</returns>
        public List<Member> GetMembers()
        {
            // Skapar och initierar ett anslutningsobjekt.
            using (var conn = CreateConnection())
            {
                try
                {
                    // Skapar det List-objekt som initialt har plats för 100 referenser till Member-objekt.
                    var members = new List<Member>(100);

                    // Skapar och initierar ett SqlCommand-objekt som används till att 
                    // exekveras specifierad lagrad procedur.
                    var cmd = new MySqlCommand("GetMembers", conn); //@TODO: Här ska en lagrad procedur som hämtar ut alla medlemmar skrivas
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Öppnar anslutningen till databasen.
                    conn.Open();

                    // Den lagrade proceduren innehåller en SELECT-sats som kan returnera flera poster varför
                    // ett SqlDataReader-objekt måste ta hand om alla poster. Metoden ExecuteReader skapar ett
                    // SqlDataReader-objekt och returnerar en referens till objektet.
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Tar reda på vilket index de olika kolumnerna har. Det är mycket effektivare att göra detta
                        // en gång för alla innan while-loopen. Genom att använda GetOrdinal behöver du inte känna till
                        // i vilken ordning de olika kolumnerna kommer, bara vad de heter.
                        var memberIdIndex = reader.GetOrdinal("MemberId");
                        var nameIndex = reader.GetOrdinal("Name");
                        var mailIndex = reader.GetOrdinal("Mail");
                        var usernameIndex = reader.GetOrdinal("Username");
                        var passwordIndex = reader.GetOrdinal("Password");

                        // Så länge som det finns poster att läsa returnerar Read true. Finns det inte fler 
                        // poster returnerar Read false.
                        while (reader.Read())
                        {
                            // Hämtar ut datat för en post. Använder GetXxx-metoder - vilken beror av typen av data.
                            // Du måste känna till SQL-satsen för att kunna välja rätt GetXxx-metod.
                            members.Add(new Member
                            {
                                MemberId = reader.GetInt32(memberIdIndex),
                                Name = reader.GetString(nameIndex),
                                Mail = reader.GetString(mailIndex),
                                Username = reader.GetString(usernameIndex),
                                Password = reader.GetString(passwordIndex)
                            });
                        }
                    }

                    // Sätter kapaciteten till antalet element i List-objektet, d.v.s. avallokerar minne
                    // som inte används.
                    members.TrimExcess();

                    // Returnerar referensen till List-objektet med referenser med Member-objekt.
                    return members;
                }
                catch
                {
                    throw new ApplicationException(GenericErrorMessage);
                }
            }
        }

        /// <summary>
        /// Hämtar en kunds kunduppgifter.
        /// </summary>
        /// <param name="memberId">En kunds kundnummer.</param>
        /// <returns>Ett Member-objekt med en kunds kunduppgifter.</returns>
        public Member GetMemberById(int memberId)
        {
            // Skapar och initierar ett anslutningsobjekt.
            using (MySqlConnection conn = CreateConnection())
            {
                try
                {
                    // Skapar och initierar ett SqlCommand-objekt som används till att 
                    // exekveras specifierad lagrad procedur.
                    MySqlCommand cmd = new MySqlCommand("app.uspGetMemberById", conn); //@TODO: SE till att proceduren finns
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Lägger till den paramter den lagrade proceduren kräver. Använder här det MINDRE effektiva 
                    // sätttet att göra det på - enkelt, men ASP.NET behöver "jobba" rätt mycket.
                    cmd.Parameters.AddWithValue("@MemberId", memberId);

                    // Öppnar anslutningen till databasen.
                    conn.Open();

                    // Den lagrade proceduren innehåller en SELECT-sats som kan returner en post varför
                    // ett SqlDataReader-objekt måste ta hand om posten. Metoden ExecuteReader skapar ett
                    // SqlDataReader-objekt och returnerar en referens till objektet.
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Om det finns en post att läsa returnerar Read true. Finns ingen post returnerar
                        // Read false.
                        if (reader.Read())
                        {
                            // Tar reda på vilket index de olika kolumnerna har. Genom att använda 
                            // GetOrdinal behöver du inte känna till i vilken ordning de olika 
                            // kolumnerna kommer, bara vad de heter.
                            int memberIdIndex = reader.GetOrdinal("MemberId");
                            int nameIndex = reader.GetOrdinal("Name");
                            int mailIndex = reader.GetOrdinal("Mail");
                            int passwordIndex = reader.GetOrdinal("Password");
                            int usernameIndex = reader.GetOrdinal("Username");

                            // Returnerar referensen till de skapade Post-objektet.
                            return new Member
                            {
                                MemberId = reader.GetInt32(memberIdIndex),
                                Name = reader.GetString(nameIndex),
                                Mail = reader.GetString(mailIndex),
                                Password = reader.GetString(passwordIndex),
                                Username = reader.GetString(usernameIndex)
                            };
                        }
                    }

                    // Istället för att returnera null kan du välja att kasta ett undatag om du 
                    // inte får "träff" på en kund. I denna applikation väljer jag att *inte* betrakta 
                    // det som ett fel om det inte går att hitta en kund. Vad du väljer är en smaksak...
                    return null;
                }
                catch
                {
                    // Kastar ett eget undantag om ett undantag kastas.
                    throw new ApplicationException(GenericErrorMessage);
                }
            }
        }

        /// <summary>
        /// Skapar en ny post i tabellen Member.
        /// </summary>
        /// <param name="member">Kunduppgifter som ska läggas till.</param>
        public void InsertMember(Member member)
        {
            // Skapar och initierar ett anslutningsobjekt.
            using (MySqlConnection conn = CreateConnection())
            {
                try
                {
                    // Skapar och initierar ett SqlCommand-objekt som används till att 
                    // exekveras specifierad lagrad procedur.
                    MySqlCommand cmd = new MySqlCommand("app.uspInsertMember", conn); //@TODO: Kolla proceduren
                    cmd.CommandType = CommandType.StoredProcedure;

                    //@TODO: kolla så att längden stämmer

                    // Lägger till de paramterar den lagrade proceduren kräver. Använder här det effektiva sätttet att
                    // göra det på - något "svårare" men ASP.NET behöver inte "jobba" så mycket.
                    cmd.Parameters.Add("@Name", MySqlDbType.VarChar, 30).Value = member.Name;
                    cmd.Parameters.Add("@Mail", MySqlDbType.VarChar, 50).Value = member.Mail;
                    cmd.Parameters.Add("@Username", MySqlDbType.VarChar, 30).Value = member.Username;
                    cmd.Parameters.Add("@Password", MySqlDbType.VarChar, 20).Value = member.Password;

                    // Den här parametern är lite speciell. Den skickar inte något data till den lagrade proceduren,
                    // utan hämtar data från den. (Fungerar ungerfär som ref- och out-prameterar i C#.) Värdet 
                    // parametern kommer att ha EFTER att den lagrade proceduren exekverats är primärnycklens värde
                    // den nya posten blivit tilldelad av databasen.
                    cmd.Parameters.Add("@MemberId", MySqlDbType.Int32, 4).Direction = ParameterDirection.Output;

                    // Öppnar anslutningen till databasen.
                    conn.Open();

                    // Den lagrade proceduren innehåller en INSERT-sats och returnerar inga poster varför metoden 
                    // ExecuteNonQuery används för att exekvera den lagrade proceduren.
                    cmd.ExecuteNonQuery();

                    // Hämtar primärnyckelns värde för den nya posten och tilldelar Member-objektet värdet.
                    member.MemberId = (int)cmd.Parameters["@MemberId"].Value;
                }
                catch
                {
                    // Kastar ett eget undantag om ett undantag kastas.
                    throw new ApplicationException(GenericErrorMessage);
                }
            }
        }

        /// <summary>
        /// Uppdaterar en kunds kunduppgifter i tabellen Member.
        /// </summary>
        /// <param name="member">Kunduppgifter som ska uppdateras.</param>
        public void UpdateMember(Member member)
        {
            // Skapar och initierar ett anslutningsobjekt.
            using (MySqlConnection conn = CreateConnection())
            {
                try
                {
                    // Skapar och initierar ett SqlCommand-objekt som används till att 
                    // exekveras specifierad lagrad procedur.
                    MySqlCommand cmd = new MySqlCommand("app.uspUpdateMember", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //@TODO: Kolla så att längden är rätt

                    // Lägger till de paramterar den lagrade proceduren kräver. Använder här det effektiva sätttet att
                    // göra det på - något "svårare" men ASP.NET behöver inte "jobba" så mycket.
                    cmd.Parameters.Add("@MemberId", MySqlDbType.Int32, 4).Value = member.MemberId;
                    cmd.Parameters.Add("@Name", MySqlDbType.VarChar, 30).Value = member.Name;
                    cmd.Parameters.Add("@Mail", MySqlDbType.VarChar, 50).Value = member.Mail;
                    cmd.Parameters.Add("@Username", MySqlDbType.VarChar, 20).Value = member.Username;
                    cmd.Parameters.Add("@Password", MySqlDbType.VarChar, 30).Value = member.Password;

                    // Öppnar anslutningen till databasen.
                    conn.Open();

                    // Den lagrade proceduren innehåller en UPDATE-sats och returnerar inga poster varför metoden 
                    // ExecuteNonQuery används för att exekvera den lagrade proceduren.
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    // Kastar ett eget undantag om ett undantag kastas.
                    throw new ApplicationException(GenericErrorMessage);
                }
            }
        }

        /// <summary>
        /// Tar bort en kunds kunduppgifter.
        /// </summary>
        /// <param name="memberId">Kunds kundnummer.</param>
        public void DeleteMember(int memberId)
        {
            // Skapar och initierar ett anslutningsobjekt.
            using (MySqlConnection conn = CreateConnection())
            {
                try
                {
                    // Skapar och initierar ett SqlCommand-objekt som används till att 
                    // exekveras specifierad lagrad procedur.
                    MySqlCommand cmd = new MySqlCommand("app.uspDeleteMember", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Lägger till den paramter den lagrade proceduren kräver. Använder här det effektiva sätttet att
                    // göra det på - något "svårare" men ASP.NET behöver inte "jobba" så mycket.
                    cmd.Parameters.Add("@MemberId", MySqlDbType.Int32, 4).Value = memberId;

                    // Öppnar anslutningen till databasen.
                    conn.Open();

                    // Den lagrade proceduren innehåller en DELETE-sats och returnerar inga poster varför metoden 
                    // ExecuteNonQuery används för att exekvera den lagrade proceduren.
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    // Kastar ett eget undantag om ett undantag kastas.
                    throw new ApplicationException(GenericErrorMessage);
                }
            }
        }

        #endregion
    }