using System.ComponentModel.DataAnnotations;

namespace LW12.Models;

public class Researcher
{
    public int ResearcherId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Position { get; set; }
    public string? Department { get; set; }
}


public class Projects
{
    public int Id { get; set; }
    public string? ProjectName { get; set; }
    public string? Description { get; set; }
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
}


public class Experiment
{
    public int Id { get; set; }
    public string? ProjectName { get; set; }
    public string? ResearcherName { get; set; }
    public string? ExperimentName { get; set; }
    public string? Result { get; set; }
    public string? Description { get; set; }
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
}