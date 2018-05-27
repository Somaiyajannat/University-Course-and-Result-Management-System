using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityCourseAndResultManagementSystemApp.Gateway;
using UniversityCourseAndResultManagementSystemApp.Models;

namespace UniversityCourseAndResultManagementSystemApp.Controllers
{
    public class CourseController : Controller
    {
        private DatabaseGateway newDatabaseGateway = new DatabaseGateway();
        // GET: Course
        

        public List<Department> LoadDeptDropBox()
        {
            List<Department> deptList = new List<Department>();
            newDatabaseGateway.Command.CommandText = "SELECT * FROM DEPARTMENT;";
            newDatabaseGateway.Connection.Open();
            SqlDataReader reader = newDatabaseGateway.Command.ExecuteReader();
            while (reader.Read())
            {
                Department newDepartment = new Department
                {
                    DeptCode = reader["DeptCode"].ToString(),
                    DeptName = reader["DeptName"].ToString()
                };
                deptList.Add(newDepartment);

            }
            newDatabaseGateway.Connection.Close();

            return deptList;

        }

        public List<string> LoadSemesterDropBox()
        {
            List<string> semList = new List<string>();
            newDatabaseGateway.Command.CommandText = "SELECT * FROM SEMESTER;";
            newDatabaseGateway.Connection.Open();
            SqlDataReader reader = newDatabaseGateway.Command.ExecuteReader();
            while (reader.Read())
            {
                semList.Add(reader["SemNo"].ToString());

            }
            newDatabaseGateway.Connection.Close();

            return semList;
        }

        public ActionResult SaveCourse(Course newCourse)
        {
            int affectedRow = 0;
            if (newCourse.DeptCode != null)
            {
                /*newDatabaseGateway.Command.CommandText = "INSERT INTO COURSE VALUES('" + newCourse.CourseCode + "','" +
                                                         newCourse.CourseName + "','" + newCourse.CourseCredit + "','" +
                                                         newCourse.CourseDes + "','" + newCourse.DeptCode + "','" +
                                                         newCourse.CourseSemester + "','Not Assigned Yet');";*/
                newDatabaseGateway.Command.CommandText = "INSERT INTO COURSE(CourseCode, CourseName, CourseCredit, CourseDes, DeptCode, CourseSemester, FACULTY) " +
                                                         "SELECT @courseCode, @courseName, @courseCredit, @courseDes, @deptCode, @courseSemester, @faculty " +
                                                         "WHERE NOT EXISTS (SELECT * FROM COURSE WHERE CourseCode = @courseCode)";
                newDatabaseGateway.Command.Parameters.AddWithValue("@courseCode", newCourse.CourseCode);
                newDatabaseGateway.Command.Parameters.AddWithValue("@courseName", newCourse.CourseName);
                newDatabaseGateway.Command.Parameters.AddWithValue("@courseCredit", newCourse.CourseCredit);
                newDatabaseGateway.Command.Parameters.AddWithValue("@courseDes", newCourse.CourseDes);
                newDatabaseGateway.Command.Parameters.AddWithValue("@deptCode", newCourse.DeptCode);
                newDatabaseGateway.Command.Parameters.AddWithValue("@courseSemester", newCourse.CourseSemester);
                newDatabaseGateway.Command.Parameters.AddWithValue("@faculty", "Not Assigned Yet");


                newDatabaseGateway.Connection.Open();
                affectedRow = newDatabaseGateway.Command.ExecuteNonQuery();
                newDatabaseGateway.Connection.Close();
                if (affectedRow == 0)
                {
                    ViewBag.message = "Course Already Exists !!!!!!";
                }
                else
                {
                    ViewBag.message = "Successfully Saved !!!!!!";
                }
            }

            ViewBag.deptList = LoadDeptDropBox();
            ViewBag.semList = LoadSemesterDropBox();
           
            return View();
        }

        public bool isChar(char ch)
        {
            if (ch >= 'a' && ch <= 'z')
                return true;
            else if (ch >= 'A' && ch <= 'Z')
                return true;
            return false;
        }

        public bool isNum(char ch)
        {
            if (ch >= '0' && ch <= '9')
                return true;
            return false;
        }

        public bool validity(Course newCourse)
        {
            // Couse Code check
            if (newCourse.CourseCode.Length != 6)
                return false;
            else
            {
                String cc = newCourse.CourseCode;
                if (isNum(cc[0]) || isNum(cc[1]) || isNum(cc[2]) || isChar(cc[3]) || isChar(cc[4]) || isChar(cc[5]))
                    return false;
            }
            //Course Name Check
            if (newCourse.CourseName == null)
                return false;
            else
            {
                String cn = newCourse.CourseName;
                for (int i = 0; i < cn.Length; i++)
                {
                    if (isNum(cn[i]))
                        return false;
                }
            }
            //Course Credit Check
            if (newCourse.CourseCredit == null)
                return false;
            else if (newCourse.CourseCredit != 3 && newCourse.CourseCredit != 4)
                return false;
            //Description Check
            if (newCourse.CourseDes == null)
            {
                return false;
            }
            else
            {
                String cd = newCourse.CourseDes;
                for (int i = 0; i < cd.Length; i++)
                {
                    if (isNum(cd[i]))
                        return false;
                }
            }
            // dept check
            if (newCourse.DeptCode == null)
                return false;
            //semester check
            if (newCourse.CourseSemester == null)
            {
                return false;
            }

            return true;
        }

        public String AddCourseCode(Course newCourse)
        {

            int affectedRow = 0;
            if (validity(newCourse))
            {
                newDatabaseGateway.Command.CommandText = "INSERT INTO COURSE(CourseCode, CourseName, CourseCredit, CourseDes, DeptCode, CourseSemester, FACULTY) " +
                                                         "SELECT @courseCode, @courseName, @courseCredit, @courseDes, @deptCode, @courseSemester, @faculty " +
                                                         "WHERE NOT EXISTS (SELECT * FROM COURSE WHERE CourseCode = @courseCode)";
                newDatabaseGateway.Command.Parameters.AddWithValue("@courseCode", newCourse.CourseCode);
                newDatabaseGateway.Command.Parameters.AddWithValue("@courseName", newCourse.CourseName);
                newDatabaseGateway.Command.Parameters.AddWithValue("@courseCredit", newCourse.CourseCredit);
                newDatabaseGateway.Command.Parameters.AddWithValue("@courseDes", newCourse.CourseDes);
                newDatabaseGateway.Command.Parameters.AddWithValue("@deptCode", newCourse.DeptCode);
                newDatabaseGateway.Command.Parameters.AddWithValue("@courseSemester", newCourse.CourseSemester);
                newDatabaseGateway.Command.Parameters.AddWithValue("@faculty", "Not Assigned Yet");


                newDatabaseGateway.Connection.Open();
                affectedRow = newDatabaseGateway.Command.ExecuteNonQuery();
                newDatabaseGateway.Connection.Close();
                if (affectedRow == 0)
                {
                    ViewBag.message = "Data Already Exists !!!!!!";
                }
                else
                {
                    ViewBag.message = "Successfully Saved !!!!!!";
                }
                return "success";
            }
            else
            {
                return "fail";
            }
            
        }

        public ActionResult AddCourse(Course newCourse)
        {
            AddCourseCode(newCourse);

            ViewBag.deptList = LoadDeptDropBox();
            ViewBag.semList = LoadSemesterDropBox();

            return View();
        }

        

        public JsonResult GetCoursesByDepartmentCode(string DeptCode)
        {
            List<Course> courses = new List<Course>();
            newDatabaseGateway.Command.CommandText = "SELECT * FROM COURSE WHERE DeptCode = '"+ DeptCode +"';";
            newDatabaseGateway.Connection.Open();
            SqlDataReader reader = newDatabaseGateway.Command.ExecuteReader();
            while (reader.Read())
            {
                Course newCourse = new Course
                {
                    CourseCode = reader["CourseCode"].ToString(),
                    CourseName = reader["CourseName"].ToString(),
                    CourseSemester = reader["CourseSemester"].ToString(),
                    CourseAssignedTo = reader["FACULTY"].ToString()
                };
                courses.Add(newCourse);
            }
            newDatabaseGateway.Connection.Close();
            return Json(courses);
        }

        public ActionResult ViewCourseSatistics()
        {
            ViewBag.deptList = LoadDeptDropBox();
            return View();
        }

        public ActionResult UnassignAllCourses()
        {
            int affectedRow = 0;
            newDatabaseGateway.Command.CommandText = "UPDATE COURSE SET FACULTY = 'Not Assigned Yet'";
            newDatabaseGateway.Connection.Open();
            affectedRow = newDatabaseGateway.Command.ExecuteNonQuery();
            newDatabaseGateway.Connection.Close();
            if (affectedRow == 0)
            {
                ViewBag.message = "Error!!";
            }
            else
            {
                ViewBag.message = "Successfully unassigned.!!!";
            }
            return View();
        }
    }
}