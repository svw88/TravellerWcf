using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;

namespace TravelerWebService
{
    
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        string conn = @"Data Source = DESKTOP-8BA0SS9; Initial Catalog = Traveler;Integrated Security = SSPI";
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "events/{Country}/{State}/{City}/{id}")]
        public List<Events> GetEvents(string Country, string State, string City, string id)
        {
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(conn))
                {
                    SqlCommand sqlCmd = new SqlCommand("Select TOP 5 * From Events Where Date >= @date AND Country = @country AND State = @state AND City = @city AND id > @id", SqlConn);
                    sqlCmd.Parameters.Add("@country", SqlDbType.NVarChar,50).Value = Country;
                    sqlCmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = State;
                    sqlCmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = City;
                    sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Today.Date;
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
                            tempEvent.Addr = (string)row[8];
                            tempEvent.Site = (string)row[9];
                            tempEvent.Image = Convert.ToBase64String((byte[])row[10]);
                            tempEvent.Date = (DateTime)row[11];
                            tempEvent.Price = (decimal)row[12];
                            tempEvent.Currency = (string)row[13];
                            tempEvent.Alias = (string)row[14];

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
                    UriTemplate = "events/{Alias}/{id}")]
        public List<Events> GetUserEvents(string alias, string id)
        {
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(conn))
                {
                    SqlCommand sqlCmd = new SqlCommand("Select TOP 5 * From Events Where Date >= @date AND Alias = @alias  AND id > @id", SqlConn);
                    sqlCmd.Parameters.Add("@alias", SqlDbType.NVarChar, 50).Value = alias;
                    sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(id);
                    sqlCmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Today.Date;
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
                        tempEvent.Addr = (string)row[8];
                        tempEvent.Site = (string)row[9];
                        tempEvent.Image = Convert.ToBase64String((byte[])row[10]);
                        tempEvent.Date = (DateTime)row[11];
                        tempEvent.Price = (decimal)row[12];
                        tempEvent.Currency = (string)row[13];
                        tempEvent.Alias = (string)row[14];

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
                    UriTemplate = "events/{Country}/{State}/{City}/{id}/{types}/{find}")]
        public List<Events> SearchEvents(string Country, string State, string City, string id, string types, string find)
        {
            if (find == "undefined")
            {
                find = string.Empty;
            }
           
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(conn))
                {
                    SqlCommand sqlCmd = new SqlCommand("Select TOP 5 * From Events Where Date >= @date AND Country = @country AND State = @state AND City = @city AND id > @id AND Type in (" + types.Replace(';', ' ') + ") AND (Name like @find OR Description like @find OR Alias = @alias)", SqlConn);
                    sqlCmd.Parameters.Add("@country", SqlDbType.NVarChar, 50).Value = Country;
                    sqlCmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = State;
                    sqlCmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = City;
                    sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCmd.Parameters.Add("@find", SqlDbType.NVarChar).Value = "%"+find+"%";
                    sqlCmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Today.Date;
                    sqlCmd.Parameters.Add("@alias", SqlDbType.NVarChar).Value = find;
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
                        tempEvent.Addr = (string)row[8];
                        tempEvent.Site = (string)row[9];
                        tempEvent.Image = Convert.ToBase64String((byte[])row[10]);
                        tempEvent.Date = (DateTime)row[11];
                        tempEvent.Price = (decimal)row[12];
                        tempEvent.Currency = (string)row[13];
                        tempEvent.Alias = (string)row[14];

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
                    SqlCommand sqlCmd = new SqlCommand("Select * From Events WHERE UserId = @userId AND Date >= @date order by Date", SqlConn);
                    sqlCmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = Guid.Parse(userId);
                    sqlCmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Today.Date;
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
                        tempEvent.Addr = (string)row[8];
                        tempEvent.Site = (string)row[9];
                        tempEvent.Image = Convert.ToBase64String((byte[])row[10]);
                        tempEvent.Date = (DateTime)row[11];
                        tempEvent.Price = (decimal)row[12];
                        tempEvent.Currency = (string)row[13];

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
                    SqlCommand sqlCmd = new SqlCommand("IF EXISTS(SELECT top 1 Id FROM Events) Insert Into Events Values (((select top 1 Id from Events order by Id desc)+1),@userId,@name,@description,@type,@country,@state,@city,@addr,@site,@img,@date,@price,@currency,(select top 1 Alias from Users where Id = @userId)) Else Insert Into Events Values (0,@userId,@name,@description,@type,@country,@state,@city,@addr,@site,@img,@date,@price,@currency,(select top 1 Alias from Users where Id = @userId)) ", SqlConn);
                    sqlCmd.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = post.Name;
                    sqlCmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = post.Description;
                    sqlCmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = post.UserId;
                    sqlCmd.Parameters.Add("@type", SqlDbType.Int).Value = Convert.ToInt32(post.Type);
                    sqlCmd.Parameters.Add("@country", SqlDbType.NVarChar, 50).Value = post.Country;
                    sqlCmd.Parameters.Add("@state", SqlDbType.NVarChar, 50).Value = post.State;
                    sqlCmd.Parameters.Add("@city", SqlDbType.NVarChar, 50).Value = post.City;
                    sqlCmd.Parameters.Add("@addr", SqlDbType.NVarChar).Value = post.Addr;
                    sqlCmd.Parameters.Add("@site", SqlDbType.NVarChar, 50).Value = post.Site;
                    sqlCmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.ParseExact(post.Date,"dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    sqlCmd.Parameters.Add("@img", SqlDbType.VarBinary).Value = Convert.FromBase64String(post.Image.Remove(0,post.Image.IndexOf(',') + 1));
                    sqlCmd.Parameters.Add("@price", SqlDbType.Money).Value = Convert.ToDecimal(post.Price);
                    sqlCmd.Parameters.Add("@currency", SqlDbType.NVarChar,2).Value = post.Currency;
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
                   UriTemplate = "remove")]
        public bool RemoveEvent(string eventId)
        {
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(conn))
                {

                    SqlConn.Open();
                    SqlCommand sqlCmd = new SqlCommand("Delete From Events Where id = @id", SqlConn);
                    sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(eventId);
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
                        sqlCmd = new SqlCommand("INSERT INTO USERS VALUES (NewID() ,@name, @surname, @alias, @email, @password, @time) ", SqlConn);
                        sqlCmd.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = post.Name;
                        sqlCmd.Parameters.Add("@surname", SqlDbType.NVarChar, 50).Value = post.Surname;
                        sqlCmd.Parameters.Add("@alias", SqlDbType.NVarChar, 50).Value = post.Alias;
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

        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "countries")]
        public List<Countries> GetCountries()
        {
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(conn))
                {
                    SqlCommand sqlCmd = new SqlCommand("Select name From Countries order by Name", SqlConn);
                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                    {
                        da.Fill(ds, "Countries");
                    }

                    var temp = new List<Countries>();
                    Countries tempCountry;


                    foreach (DataRow row in ds.Tables[0].Rows)
                    {

                        tempCountry = new Countries();
                        tempCountry.Name = (string)row[0];


                        temp.Add(tempCountry);
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
                   UriTemplate = "states/{name}")]
        public List<States> GetStates(string name)
        {
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(conn))
                {
                    SqlCommand sqlCmd = new SqlCommand("Select States.name From States Join Countries on States.country_id = Countries.id  Where Countries.Name = @name order by States.Name", SqlConn);
                    sqlCmd.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = name;
                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                    {
                        da.Fill(ds, "States");
                    }

                    var temp = new List<States>();
                    States tempCountry;


                    foreach (DataRow row in ds.Tables[0].Rows)
                    {

                        tempCountry = new States();
                        tempCountry.Name = (string)row[0];


                        temp.Add(tempCountry);
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
                  UriTemplate = "cities/{name}")]
        public List<Cities> GetCities(string name)
        {
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(conn))
                {
                    SqlCommand sqlCmd = new SqlCommand("Select Cities.name From Cities Join States on Cities.state_id = States.id  Where States.Name = @name order by Cities.Name", SqlConn);
                    sqlCmd.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = name;
                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                    {
                        da.Fill(ds, "Cities");
                    }

                    var temp = new List<Cities>();
                    Cities tempCountry;


                    foreach (DataRow row in ds.Tables[0].Rows)
                    {

                        tempCountry = new Cities();
                        tempCountry.Name = (string)row[0];


                        temp.Add(tempCountry);
                    }


                    return temp;
                }
            }
            catch (Exception ex)
            {
                return null;
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
            public string Addr { get; set; }
            [DataMember(Order = 9)]
            public string Site { get; set; }
            [DataMember(Order = 10)]
            public string Image { get; set; }
            [DataMember(Order = 11)]
            public DateTime Date { get; set; }
            [DataMember(Order = 12)]
            public decimal Price { get; set; }
            [DataMember(Order = 13)]
            public string Currency { get; set; }
            [DataMember(Order = 14)]
            public string Alias { get; set; }
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
            public string Price { get; set; }
            [DataMember(Order = 4)]
            public string Currency { get; set; }
            [DataMember(Order = 5)]
            public string Country { get; set; }
            [DataMember(Order = 6)]
            public string State { get; set; }
            [DataMember(Order = 7)]
            public string City { get; set; }
            [DataMember(Order = 8)]
            public string Addr { get; set; }
            [DataMember(Order = 9)]
            public string Site { get; set; }
            [DataMember(Order = 10)]
            public string Date { get; set; }
            [DataMember(Order = 11)]
            public string Image { get; set; }
            [DataMember(Order = 12)]
            public Guid UserId { get; set; }
            [DataMember(Order = 13)]
            public string Alias { get; set; }

        }

        [DataContract]
        public class User
        {
            [DataMember(Order = 0)]
            public string Name { get; set; }
            [DataMember(Order = 1)]
            public string Surname { get; set; }
            [DataMember(Order = 2)]
            public string Alias { get; set; }
            [DataMember(Order = 3)]
            public string Email { get; set; }
            [DataMember(Order = 4)]
            public string Password { get; set; }
        }

        [DataContract]
        public class Countries
        {
            [DataMember(Order = 0)]
            public string Name { get; set; }
        }

        [DataContract]
        public class States
        {
            [DataMember(Order = 0)]
            public string Name { get; set; }
        }

        [DataContract]
        public class Cities
        {
            [DataMember(Order = 0)]
            public string Name { get; set; }
        }

    }
}
