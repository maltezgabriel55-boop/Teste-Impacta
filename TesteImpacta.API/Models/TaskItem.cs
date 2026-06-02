namespace TesteImpacta.API.Models;

public class TaskItem
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public string Status { get; set; } = "Pendente";

    public DateTime DataCriacao { get; set; }
}