using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string JPGFileLocation = System.Configuration.ConfigurationManager.AppSettings["JPGFileLocation"];
            string MovieProjectDirectory = System.Configuration.ConfigurationManager.AppSettings["MovieProjectDirectory"];

            if (CopyFilesIntoMonthYearDirectories())
                Utilities.CopyFilesIntoDirectories(JPGFileLocation);
            if (BuildMovieProject())
                Utilities.BuildMovieProjects(JPGFileLocation, MovieProjectDirectory);
        }

        private static bool BuildMovieProject()
        {
            return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["BuildMovieProject"]);
        }

        private static bool CopyFilesIntoMonthYearDirectories()
        {
            return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["CopyFilesIntoMonthYearDirectories"]);
        }



    }
}
