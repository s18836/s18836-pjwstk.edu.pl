using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw3.DTOs.Requests;
using cw3.DTOs.Responses;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace cw3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {


        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
          
                     

            using(var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18836;Integrated Security=True;MultipleActiveResultSets=True"))
            using(var com = new SqlCommand())
            {

                int id_enrollment = 0;

                com.Connection = con;
                con.Open();

                var tran = con.BeginTransaction();
                com.Transaction = tran;

               // try
                {
                   


                    com.CommandText = "USE [2019SBD]; SELECT * FROM STUDIES WHERE NAME = @sname;";
                    com.Parameters.AddWithValue("sname", request.Studies);
                    var dr = com.ExecuteReader();

                    
                    if (!dr.Read())
                    {
                        tran.Rollback();
                        return BadRequest("STUDIA NIE ISTNIEJA");
                    }
                    int id_studies = (int)dr["IdStudy"];
                    
                    dr.Close();



                   
                    com.CommandText = "USE [2019SBD];SELECT MAX(IDENROLLMENT) AS IdEnrollment FROM ENROLLMENT WHERE (ENROLLMENT.IdStudy = (SELECT STUDIES.IdStudy FROM STUDIES WHERE Studies.NAME = @s_name)) AND ENROLLMENT.Semester = 1;";
                    com.Parameters.AddWithValue("s_name", request.Studies);
                    dr = com.ExecuteReader();
                    
                    if (!dr.Read())  // baza danych zwarca null??
                    {
                        
                        dr.Close();


                        com.CommandText = "SELECT IDSTUDY FROM STUDIES WHERE NAME = @n_study;";
                        com.Parameters.AddWithValue("@n_study", request.Studies);
                        dr = com.ExecuteReader();
                        int id_studiow_dodawanych = (int)(dr["IDSTUDY"]);
                        dr.Close();

                        com.CommandText = "SELECT MAX(IDENROLLMENT) AS MAX_ID FROM ENROLLMENT;";
                        dr = com.ExecuteReader();
                        int max_id_enrollment = (int)(dr["MAX_ID"]);
                        int nowy_id = max_id_enrollment + 1;
                        dr.Close();

                        com.CommandText = "INSERT INTO ENROLLMENT VALUES (@n_id, 1, @id_s, @start_date);";
                        com.Parameters.AddWithValue("n_id", nowy_id);
                        com.Parameters.AddWithValue("id_s", id_studiow_dodawanych);
                        com.Parameters.AddWithValue("start_date", DateTime.Now);
                        com.ExecuteNonQuery();
                        dr.Close();

                        id_enrollment = id_studiow_dodawanych;

                    }

                    else {
                      
                        id_enrollment = (int)(dr["IdEnrollment"]);
                        dr.Close();
                    }

                    

                    DateTime bd = Convert.ToDateTime(request.BirthDate);

                    com.CommandText = "USE [2019SBD]; INSERT INTO  STUDENT VALUES (@Index, @Fname, @Lname, @BD, @idE)";
                    com.Parameters.AddWithValue("Index", request.IndexNumber);
                    com.Parameters.AddWithValue("Fname", request.FirstName);
                    com.Parameters.AddWithValue("Lname", request.LastName);
                    com.Parameters.AddWithValue("BD", bd);
                    com.Parameters.AddWithValue("idE", id_enrollment);


                    com.ExecuteNonQuery();
                    tran.Commit();

                }//catch(SqlException e)
               // {
                //    tran.Rollback();
                //    return BadRequest("ROLLBACK");
                //}
                    


            }


            return Ok();
        }  
    }
}