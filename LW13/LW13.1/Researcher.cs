using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LW13._1
{
    public class Researcher
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }
    }
    public class Project
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
    }

    public class Experiment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? ProjectName { get; set; }
        public string? ResearcherName { get; set; }
        public string? ExperimentName { get; set; }
    }
}