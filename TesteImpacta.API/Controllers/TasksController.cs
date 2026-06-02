using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteImpacta.API.Data;
using TesteImpacta.API.Models;

namespace TesteImpacta.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
    {
        var tasks = await _context.Tasks
            .OrderByDescending(t => t.DataCriacao)
            .ToListAsync();

        return Ok(tasks);
    }

    // GET: api/tasks/1
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetById(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
            return NotFound(new
            {
                message = "Tarefa não encontrada."
            });

        return Ok(task);
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create(TaskItem task)
    {
        task.DataCriacao = DateTime.Now;

        if (string.IsNullOrWhiteSpace(task.Status))
            task.Status = "Pendente";

        _context.Tasks.Add(task);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = task.Id },
            task);
    }

    // PUT: api/tasks/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TaskItem task)
    {
        if (id != task.Id)
            return BadRequest(new
            {
                message = "ID informado é diferente do ID da tarefa."
            });

        var existingTask = await _context.Tasks.FindAsync(id);

        if (existingTask == null)
            return NotFound(new
            {
                message = "Tarefa não encontrada."
            });

        existingTask.Titulo = task.Titulo;
        existingTask.Descricao = task.Descricao;
        existingTask.Status = task.Status;

        await _context.SaveChangesAsync();

        return Ok(existingTask);
    }

    // DELETE: api/tasks/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
            return NotFound(new
            {
                message = "Tarefa não encontrada."
            });

        _context.Tasks.Remove(task);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Tarefa removida com sucesso."
        });
    }
}