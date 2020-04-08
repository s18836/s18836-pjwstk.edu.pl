using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw3.DAL;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers
{

    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }




        [HttpGet("{IndexNumber}")]
        public IActionResult GetStudents(string indexNumber)
        {
            ListaStudentow l1 = new ListaStudentow();
           
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18836;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
               
                com.Connection = client;
                com.CommandText = "USE [2019SBD]; SELECT * FROM STUDENT where STUDENT.IndexNumber = @index";

                com.Parameters.AddWithValue("index", indexNumber);
                client.Open();
                
                var dr = com.ExecuteReader();

                while (dr.Read())
                {
                  
                    {
                        var st = new Student();

                        st.FirstName = (dr["FirstName"].ToString());
                        st.LastName = (dr["LastName"].ToString());
                        st.IndexNumber = (dr["IndexNumber"].ToString());
                        st.BirthDate = ((DateTime)dr["BirthDate"]);
                        st.IdEnrollment = (dr["IdEnrollment"].ToString());
                        l1.dodaj(st);
                    }
                }
                
            }
            

            
            return Ok(l1.getlist());
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            //... add to database
            //... generating index number

            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent()
        {
            return Ok("Aktualizacja ukonczona");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent()
        {
            return Ok("Usuwanie zakonczone");
        }

    }
}