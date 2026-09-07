namespace ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientsList;

public class PatientsFilterDTO
{
    public int Page { get; set; } = 1;
    public int RecordsPerPage { get; set; } = 10;
    public string? Name { get; set; }
    public string? Email { get; set; }
}
