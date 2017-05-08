using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TravelerWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        string conn = @"Data Source = Database; Initial Catalog = Traveler;Integrated Security = SSPI";
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "events/{Country}/{State}/{City}")]
        public List<Events> GetEvents(string Country, string State, string City)
        {
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(conn))
                {
                    SqlCommand sqlCmd = new SqlCommand("Select * From Events", SqlConn);
                    sqlCmd.Parameters.Add("@country", SqlDbType.NVarChar,50).Value = "";
                    sqlCmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = "";
                    sqlCmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = "";
                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                    {
                        da.Fill(ds, "Events");
                    }

                    var temp = new List<Events>();
                    Events tempEvent;
                    

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            
                            tempEvent = new Events();
                            tempEvent.Id = (Int64)row[0];
                            tempEvent.UserId = (Guid)row[1];
                            tempEvent.Name = (string)row[2];
                            tempEvent.Description = (string)row[3];
                            tempEvent.Type = (Int16)row[4];
                            tempEvent.Country = (string)row[5];
                            tempEvent.State = (string)row[6];
                            tempEvent.City = (string)row[7];
                            tempEvent.Site = (string)row[8];
                            tempEvent.Image = Convert.ToBase64String((byte[])row[9]);
                            tempEvent.Date = (DateTime)row[10];
                            tempEvent.Price = (decimal)row[11];

                            temp.Add(tempEvent);
                        }
                    

                    return temp;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "myevents/{userId}")]
        public List<Events> GetMyEvents(string userId)
        {
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(conn))
                {
                    SqlCommand sqlCmd = new SqlCommand("Select * From Events WHERE UserId = @userId", SqlConn);
                    sqlCmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = Guid.Parse(userId);
                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                    {
                        da.Fill(ds, "Events");
                    }

                    var temp = new List<Events>();
                    Events tempEvent;


                    foreach (DataRow row in ds.Tables[0].Rows)
                    {

                        tempEvent = new Events();
                        tempEvent.Id = (Int64)row[0];
                        tempEvent.UserId = (Guid)row[1];
                        tempEvent.Name = (string)row[2];
                        tempEvent.Description = (string)row[3];
                        tempEvent.Type = (Int16)row[4];
                        tempEvent.Country = (string)row[5];
                        tempEvent.State = (string)row[6];
                        tempEvent.City = (string)row[7];
                        tempEvent.Site = (string)row[8];
                        tempEvent.Image = Convert.ToBase64String((byte[])row[9]);
                        tempEvent.Date = (DateTime)row[10];
                        tempEvent.Price = (decimal)row[11];

                        temp.Add(tempEvent);
                    }


                    return temp;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebInvoke(Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "create")]
        public bool CreateEvent(Event post)
        {
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(conn))
                {

                    SqlConn.Open();
                    SqlCommand sqlCmd = new SqlCommand("IF EXISTS(SELECT top 1 Id FROM Events) Insert Into Events Values (((select top 1 Id from Events order by Id desc)+1),@userId,@name,'',@type,'','','','',@img,@date,20) Else Insert Into Events Values (0,@userId,@name,'',@type,'','','','',@img,@date,20) ", SqlConn);
                    sqlCmd.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = post.Name;
                    sqlCmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = post.UserId;
                    sqlCmd.Parameters.Add("@type", SqlDbType.Int).Value = Convert.ToInt32(post.Type);
                    sqlCmd.Parameters.Add("@date", SqlDbType.DateTime).Value = Convert.ToDateTime(post.Date);
                    sqlCmd.Parameters.Add("@img", SqlDbType.VarBinary).Value = Convert.FromBase64String(post.Image.Remove(0,post.Image.IndexOf(',') + 1));
                    sqlCmd.ExecuteNonQuery();

                    SqlConn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [WebInvoke(Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "register")]
        public bool Register(User post)
        {
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(conn))
                {

                    SqlCommand sqlCmd = new SqlCommand("SELECT * FROM USERS WHERE Email = @email", SqlConn);
                    sqlCmd.Parameters.Add("@email", SqlDbType.NVarChar, 50).Value = post.Email;
                    sqlCmd.Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = post.Password;
                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                    {
                        da.Fill(ds, "Users");
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return false;
                    }
                    else
                    {
                        SqlConn.Open();
                        sqlCmd = new SqlCommand("INSERT INTO USERS VALUES (NewID(), @email, @password, @time) ", SqlConn);
                        sqlCmd.Parameters.Add("@email", SqlDbType.NVarChar, 50).Value = post.Email;
                        sqlCmd.Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = post.Password;
                        sqlCmd.Parameters.Add("@time", SqlDbType.NVarChar, 50).Value = Convert.ToString(DateTime.Today);
                        sqlCmd.ExecuteNonQuery();
                        SqlConn.Close();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "login/{Email}/{Password}")]
        public string Login(string Email, string Password)
        {
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(conn))
                {
                    SqlCommand sqlCmd = new SqlCommand("Select * From Users WHERE Email = @email", SqlConn);
                    sqlCmd.Parameters.Add("@email", SqlDbType.NVarChar, 50).Value = Email;
                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                    {
                        da.Fill(ds, "Events");
                    }

                    var temp = new User();
                    temp.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    temp.Password = ds.Tables[0].Rows[0]["Password"].ToString();

                    if (temp.Password == Password)
                    {
                        return ds.Tables[0].Rows[0]["Id"].ToString(); ;
                    }
                    else
                    {
                        return string.Empty;
                    }

                   
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        [DataContract]
        public class Events
        {
            [DataMember(Order = 0)]
            public Int64 Id { get; set; }
            [DataMember(Order = 1)]
            public Guid UserId { get; set; }
            [DataMember(Order = 2)]
            public string Name { get; set; }
            [DataMember(Order = 3)]
            public string Description { get; set; }
            [DataMember(Order = 4)]
            public Int16 Type { get; set; }
            [DataMember(Order = 5)]
            public string Country { get; set; }
            [DataMember(Order = 6)]
            public string State { get; set; }
            [DataMember(Order = 7)]
            public string City { get; set; }
            [DataMember(Order = 8)]
            public string Site { get; set; }
            [DataMember(Order = 9)]
            public string Image { get; set; }
            [DataMember(Order = 10)]
            public DateTime Date { get; set; }
            [DataMember(Order = 11)]
            public decimal Price { get; set; }
        }

        [DataContract]
        public class Event
        {
            [DataMember(Order = 0)]
            public string Name { get; set; }
            [DataMember(Order = 1)]
            public string Description { get; set; }
            [DataMember(Order = 2)]
            public Int16 Type { get; set; }            
            [DataMember(Order = 3)]
            public string Date { get; set; }
            [DataMember(Order = 4)]
            public string Image { get; set; }
            [DataMember(Order = 5)]
            public Guid UserId { get; set; }

        }

        [DataContract]
        public class User
        {
            [DataMember(Order = 0)]
            public string Email { get; set; }
            [DataMember(Order = 1)]
            public string Password { get; set; }
        }

    }
}
