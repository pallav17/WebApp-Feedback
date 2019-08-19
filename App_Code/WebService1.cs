using ChoETL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Script.Services;
using System.Configuration;
using System.Net.Mail;


namespace Official

{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        string connectionstring = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
        SqlCommand cmd = new SqlCommand();




        public WebService1()
        {


        }

        [WebMethod]
        public string HelloWorld()

        {
            return "Hello World";
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getEmployee()
        {


            StringBuilder sb = new StringBuilder();

            string connectionstring = @"Data Source = P01156006; Initial Catalog = practice; Integrated Security = SSPI";
            //  string connectionstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True";

            using (var conn = new SqlConnection(connectionstring))
            //using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connectstr"].ToString())) ;
            {
                Context.Response.ContentType = "application/json";
                conn.Open();
                //var comm = new SqlCommand("SELECT * FROM employee where id = 2", conn);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "GetEmployees";
                SqlDataReader rdr = cmd.ExecuteReader();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                // SqlDataAdapter adap = new SqlDataAdapter(rdr);

                DataTable dt = new DataTable("employee");
                //rdr.Fill(dt);

                using (var parser = new ChoJSONWriter(sb))

                    parser.Write(rdr);
                String k = sb.ToString();

                Context.Response.Write(k);
                // return sb.ToString();
                //this.Context.Response.ContentType = "application/json; charset=utf-8";
                //this.Context.Response.Write(serializer.Serialize(k));


            }

            //this.Context.Response.ContentType = "application/json; charset=utf-8";
            //   this.Context.Response.Write(json);
            //    string sql = "SELECT * FROM employee";
            //    SqlDataAdapter da = new SqlDataAdapter(sql, ConfigurationManager.ConnectionStrings["Connectstr"].ToString());
            //    DataSet ds = new DataSet();

            //    DataTable table = new DataTable();


            //    da.Fill(ds);
            //    var empList = ds.Tables[0].AsEnumerable().Select(dataRow => new Employee { Name = dataRow.Field<string>("Name") }).ToList()
            //    return JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented);
            //}



        }



        [WebMethod(EnableSession = true)]
        public Boolean InsertFeedbackDataNew(string Subject, string Recepient_Email, string Description, string Suggestion, string Email, int Rating )
        {
            SmtpClient smtp;
           
            // string connectionstring = @"Data Source = P01156006; Initial Catalog = practice; Integrated Security = SSPI";

            // string connectionstring = @"Data Source = WS001892; Initial Catalog = OfficialDb; User Id = feedback_sa; password = sa123; Integrated Security = True; MultipleActiveResultSets=True ";
            using (SqlConnection con = new SqlConnection(connectionstring))

            {
                //con.Open();
                //SqlCommand cmd1 = new SqlCommand("select count(Email) from tblFeedback where Email=@Email and Recepient_Email = @Recepient_Email ", con);
                //cmd1.Parameters.AddWithValue("@Email", Email);
                //cmd1.Parameters.AddWithValue("@Recepient_Email", Recepient_Email);
                //int d = (Int32)cmd1.ExecuteScalar();
                //if (d == 0)
                //{
                //    con.Close();
                con.Open();

               string FeedbackDate = DateTime.Today.ToString("MM/dd/yyyy");
               string FeedbackTime = DateTime.Now.ToString("HH:mm:ss");
                //string Description = pr + " Improvement:    " + nr;
                cmd = new SqlCommand("insert into tblFeedback(Subject, Recepient_Email,Description,Suggestion,Rating,Email, FeedbackDate, FeedbackTime)values(@Subject,@Recepient_Email,@Description,@Suggestion,@Rating,@Email,'"+FeedbackDate+ "','" + FeedbackTime + "'  )", con);
                cmd.Parameters.AddWithValue("@Subject", Subject);
                cmd.Parameters.AddWithValue("@Recepient_Email", Recepient_Email);
                cmd.Parameters.AddWithValue("@Description", Description);
                cmd.Parameters.AddWithValue("@Suggestion", Suggestion);
                cmd.Parameters.AddWithValue("@Rating", Rating);

                cmd.Parameters.AddWithValue("@Email", Email);
                int i = 0;
                i = cmd.ExecuteNonQuery();
              
                using (MailMessage mm = new MailMessage("FeedbackApp@schaeffler.com", Recepient_Email))
                {
                    mm.Subject = "You have a new Feedback waiting";
                    mm.Body = string.Format("Hello There, <br /> You have a new Feedback on your name. Please login to the Feedback App and check it. Link to the App: https://bit.ly/2WeSzGY");
                    mm.IsBodyHtml = true;
                    smtp = new SmtpClient();
                    smtp.Host = "smtp.na.ina.com";

                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = credentials;
                    smtp.Send(mm);
                    //WriteToFile("Email sent successfully to: " + name1 + " " + email1);



                    if (i > 0)
                    {
                        return true;
                   
                    }
                    else
                    {
                        return false;
                    }

                }

                con.Close();
                //else
                //{
                //    return false;
                //}
            }

        }

        [WebMethod(EnableSession = true)]
        public void GetAllUserData()
        {

            // string connectionstring = @"Data Source = P01156006; Initial Catalog = practice; Integrated Security = SSPI";
            using (SqlConnection con = new SqlConnection(connectionstring))
            {

            string email1 = Session["stroreEmail"].ToString();

            List<WebService1class> listUser = new List<WebService1class>();
            SqlCommand cmd = new SqlCommand("select FirstName,LastName,Office,Email,PhoneNo,IsActive from tblUser where IsActive='True' and Email !='" + email1 + "'", con);
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                WebService1class user = new WebService1class();
                user.FirstName = sdr["FirstName"].ToString();
                user.LastName = sdr["LastName"].ToString();
                user.Office = sdr["Office"].ToString();
                user.Email = sdr["Email"].ToString();
                user.PhoneNo = sdr["PhoneNo"].ToString();
                user.IsActive = Convert.ToBoolean(sdr["IsActive"]);
                listUser.Add(user);
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(listUser));
            con.Close();

        }

    
    }

        [WebMethod(EnableSession = true)]
        public void TokenTest2AU(string Email, string Token)
        {

            // string connectionstring = @"Data Source = P01156006; Initial Catalog = practice; Integrated Security = SSPI";
            using (SqlConnection con = new SqlConnection(connectionstring))
            {

                // string email1 = Session["stroreEmail"].ToString();

                string email1 = Email;
                string Error = "Sorry You are not an authorized user";


                List<WebService1class> listUser = new List<WebService1class>();
                SqlCommand cmd = new SqlCommand("select FirstName,LastName,Office,Email,PhoneNo,IsActive from tblUser where IsActive='True' and Email !='" + email1 + "'", con);
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    WebService1class user = new WebService1class();
                    user.FirstName = sdr["FirstName"].ToString();
                    user.LastName = sdr["LastName"].ToString();
                    user.Office = sdr["Office"].ToString();
                    user.Email = sdr["Email"].ToString();
                    user.PhoneNo = sdr["PhoneNo"].ToString();
                    user.IsActive = Convert.ToBoolean(sdr["IsActive"]);
                    listUser.Add(user);
                }

                if (Token == "af9bce267343ad72bd6abe7aff58edf2")
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Context.Response.Write(js.Serialize(listUser));
                    con.Close();
                }

                else
                {
                    Context.Response.Write(Error);
                    con.Close();
                }



            }


        }




        [WebMethod(EnableSession = true)]
        public void GetFeedBackDetailNew()
        {
            
            // string connectionstring = @"Data Source = P01156006; Initial Catalog = practice; Integrated Security = SSPI";
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
               
                
                    con.Open();
                    string email1 = Session["stroreEmail"].ToString();
                    List<WebService1Class> listFeedback = new List<WebService1Class>();

                    SqlCommand cmd = new SqlCommand("select f.Description, f.FeedbackDate, f.Suggestion,f.Rating,f.Subject,u.FirstName,u.LastName from tblFeedback f,tblUser u where f.Email=u.Email and f.Recepient_Email='" + email1 + "' ", con);
                    // SqlCommand cmd = new SqlCommand("select * from tblFeedback ", con);
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        WebService1Class feedback = new WebService1Class();
                        feedback.Description = sdr["Description"].ToString();
                        feedback.Suggestion = sdr["Suggestion"].ToString();
                    feedback.Rating = Convert.ToInt32(sdr["Rating"]);
                        feedback.Subject = sdr["Subject"].ToString();
                       feedback.FeedbackDate = sdr["FeedbackDate"].ToString();


                    feedback.FirstName = sdr["FirstName"].ToString();
                        feedback.LastName = sdr["LastName"].ToString();
                        listFeedback.Add(feedback);
                    
                    }
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Context.Response.Write(js.Serialize(listFeedback));
                    con.Close();
                }    
        }



        [WebMethod(EnableSession = true)]
        public void TokenTest1DF(string Email, string Token)
        {

            // string connectionstring = @"Data Source = P01156006; Initial Catalog = practice; Integrated Security = SSPI";
            using (SqlConnection con = new SqlConnection(connectionstring))
            {


                con.Open();
                //string email1 = Session["stroreEmail"].ToString();

                string email1 = Email;
                string Error = "Sorry You are not an authorized user";
           
                List<WebService1Class> listFeedback = new List<WebService1Class>();

                SqlCommand cmd = new SqlCommand("select f.Description, f.FeedbackDate, f.Suggestion,f.Rating,f.Subject,u.FirstName,u.LastName from tblFeedback f,tblUser u where f.Email=u.Email and f.Recepient_Email='" + email1 + "' ", con);
                // SqlCommand cmd = new SqlCommand("select * from tblFeedback ", con);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    WebService1Class feedback = new WebService1Class();
                    feedback.Description = sdr["Description"].ToString();
                    feedback.Suggestion = sdr["Suggestion"].ToString();
                    feedback.Rating = Convert.ToInt32(sdr["Rating"]);
                    feedback.Subject = sdr["Subject"].ToString();
                    feedback.FeedbackDate = sdr["FeedbackDate"].ToString();


                    feedback.FirstName = sdr["FirstName"].ToString();
                    feedback.LastName = sdr["LastName"].ToString();
                    listFeedback.Add(feedback);

                }

                if ( Token == "af9bce267343ad72bd6abe7aff58edf2" )
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Context.Response.Write(js.Serialize(listFeedback));
                    con.Close();
                }

                else
                {
                    Context.Response.Write(Error);
                    con.Close();
                }
  

               
            }

            
        }


        [WebMethod]
        public string Otp()
        {
            Random generator = new Random();
            string number;

            number = generator.Next(1, 10000).ToString("D4");
            return number;

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Boolean getLogin(string Email, string Password)
        {
          //  string connectionstring = @"Data Source = P01156006; Initial Catalog = practice; Integrated Security = SSPI";
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                con.Open();
                //Execute command
               SqlCommand cmd = new SqlCommand("select count(Email) from tblUser where Email=@Email and Password=@Password and IsActive='True'", con);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Password", Password);
               
                Session["stroreEmail"] = Email;
               
                int d = (Int32)cmd.ExecuteScalar();
                if (d == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                con.Close();
            }
        }


        [WebMethod]
        public Boolean InsertUserData(string FirstName, string LastName, string Office, string Email, string Password, Boolean IsActive, Boolean IsDelete)
        {
           
          //  string connectionstring = @"Data Source = P01156006; Initial Catalog = practice; Integrated Security = SSPI";
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string AccessOtp;
                {

                    Random generator = new Random();
                    AccessOtp = generator.Next(1, 10000).ToString("D4");

                }

                con.Open();
                SqlCommand cmd1 = new SqlCommand("select count(Email) from tblUser where Email=@Email", con);
                cmd1.Parameters.AddWithValue("@Email", Email);
                int d = (Int32)cmd1.ExecuteScalar();
                if (d == 0)
                {
                    using (MailMessage mm = new MailMessage("FeedbackApp@schaeffler.com", @Email))
                    {
                        mm.Subject = "Feedback App registeration Access code";
                        mm.Body = string.Format("<b>Your Access code is: </b>{0}", AccessOtp);

                        mm.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.na.ina.com";
                      
                        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = credentials;
                        smtp.Send(mm);
                        //WriteToFile("Email sent successfully to: " + name1 + " " + email1);
                        
                        
                    }
                    con.Close();
                    con.Open();
                   
                    
                    //SqlCommand cmd = new SqlCommand("insert into tblUser(FirstName,LastName,Office,Email,Password,PhoneNo,OtpNo,IsActive,IsDelete)values(@FirstName,@LastName,@Office,@Email,@Password,@PhoneNo,'" + AccessOtp.ToString() + "',@IsActive,@IsDelete)", con);
                    SqlCommand cmd = new SqlCommand("insert into tblUser(FirstName,LastName,Office,Email,Password,OtpNo,IsActive,IsDelete)values(@FirstName,@LastName,@Office,@Email,@Password,'" + AccessOtp.ToString() + "',@IsActive,@IsDelete)", con);
                  //  SqlCommand cmd = new SqlCommand("insert into tblUser(FirstName,LastName,Office,Email,Password,OtpNo,IsActive,IsDelete)values(@FirstName,@LastName,@Office,@Email,@Password,'" + AccessOtp.ToString() + "',' False ',@IsDelete)", con);

                    cmd.Parameters.AddWithValue("@FirstName", FirstName);
                    cmd.Parameters.AddWithValue("@LastName", LastName);
                    cmd.Parameters.AddWithValue("@Office", Office);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    //cmd.Parameters.AddWithValue("@PhoneNo", PhoneNo);
                    cmd.Parameters.AddWithValue("@IsActive", IsActive);
                    cmd.Parameters.AddWithValue("@IsDelete", IsDelete);
                    int j = 0;
                    j = cmd.ExecuteNonQuery();
                    if (j > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
                con.Close();

            }

        }


        [WebMethod]
        public Boolean VerifyOtpNumber(string OtpNo)
        {

           // string connectionstring = @"Data Source = P01156006; Initial Catalog = practice; Integrated Security = SSPI";
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                con.Open();
                //Execute command
               SqlCommand cmd = new SqlCommand("select count(OtpNo) from tblUser where OtpNo=@OtpNo", con);
                cmd.Parameters.AddWithValue("@OtpNo", OtpNo);
                int d = (Int32)cmd.ExecuteScalar();
                if (d == 1)
                {
                    con.Close();
                    con.Open();
                    SqlCommand cmd1 = new SqlCommand("update tblUser set IsActive='True' where OtpNo=@OtpNo1", con);
                    cmd1.Parameters.AddWithValue("@OtpNo1", OtpNo);
                    int u = cmd1.ExecuteNonQuery();
                    return true;

                }
                else
                {
                    return false;
                }
                con.Close();
            }

        }

        [WebMethod]
        public Boolean VerifyforgotOtpNumber(string OtpNo, string Email)
        {
            con.Open();
            //Execute command

            cmd = new SqlCommand("select count(OtpNo) from tblUser where OtpNo=@OtpNo and Email=@Email and Password=''", con);
            cmd.Parameters.AddWithValue("@OtpNo", OtpNo);
            cmd.Parameters.AddWithValue("@Email", Email);
            int d = (Int32)cmd.ExecuteScalar();
            if (d == 1)
            {
                con.Close();
                con.Open();
                SqlCommand cmd1 = new SqlCommand("update tblUser set IsActive='True' where OtpNo=@OtpNo1", con);
                cmd1.Parameters.AddWithValue("@OtpNo1", OtpNo);
                int u = cmd1.ExecuteNonQuery();
                return true;

            }
            else
            {
                return false;
            }
            con.Close();
        }

        [WebMethod(EnableSession = true)]
        public Boolean ForgotPassMain(string Password)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                con.Open();
                string femail1 = Session["femail"].ToString();
                SqlCommand cmd = new SqlCommand("update tblUser set Password=@Password where Email='" + femail1 + "' ", con);
                cmd.Parameters.AddWithValue("@Password", Password);
                int i = 0;
                i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                con.Close();
            }


        }



        [WebMethod(EnableSession = true)]
        public Boolean ForgotPassword(string Email)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                con.Open();
                //Execute command
                cmd = new SqlCommand("select count(Email) from tblUser where Email=@Email and IsActive='True'", con);
                cmd.Parameters.AddWithValue("@Email", Email);
                int da = (Int32)cmd.ExecuteScalar();
                if (da == 1)
                {
                    SqlCommand cmd2 = new SqlCommand("update tblUser set Password='' where Email=@Email1", con);
                    cmd2.Parameters.AddWithValue("@Email1", Email);
                    int c2 = cmd2.ExecuteNonQuery();
                    string AccessOtp;
                    {

                        Random generator = new Random();
                        AccessOtp = generator.Next(1, 10000).ToString("D4");

                    }
                    using (MailMessage mm = new MailMessage("FeedbackApp@schaeffler.com", @Email))
                    {
                        mm.Subject = "Access code for password reset";
                        mm.Body = string.Format("<b>Your  Access code is </b>{0}", AccessOtp);

                        mm.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.na.ina.com";

                        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = credentials;
                        smtp.Send(mm);
                        //WriteToFile("Email sent successfully to: " + name1 + " " + email1);
                    }
                    
                    SqlCommand cmd3 = new SqlCommand("update tblUser set OtpNo='" + AccessOtp + "' where Email=@Email2", con);
                    cmd3.Parameters.AddWithValue("@Email2", Email);

                    int c3 = cmd3.ExecuteNonQuery();
                    if (c3 > 0)
                    {
                        Session["femail"] = Email;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    con.Close();
                }



            }
            return false;

        }


    }
}
        