import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TaskService, TaskItem } from './services/task';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  tarefas: TaskItem[] = [];

  tarefa = {
    titulo: '',
    descricao: '',
    status: 'Pendente'
  };

  editando = false;
  idEditando: number | null = null;
  mensagem = '';

  constructor(
    private taskService: TaskService,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.carregarTarefas();
  }

  carregarTarefas(): void {
    this.taskService.getAll().subscribe({
      next: (dados) => {
        this.tarefas = dados;
        this.cd.detectChanges();
      },
      error: () => {
        this.mensagem = 'Erro ao carregar tarefas.';
      }
    });
  }

  salvar(): void {
    if (!this.tarefa.titulo.trim()) {
      this.mensagem = 'Informe o título da tarefa.';
      return;
    }

    if (this.editando && this.idEditando !== null) {
      const tarefaAtualizada: TaskItem = {
        id: this.idEditando,
        titulo: this.tarefa.titulo,
        descricao: this.tarefa.descricao,
        status: this.tarefa.status,
        dataCriacao: new Date().toISOString()
      };

      this.taskService.update(tarefaAtualizada).subscribe({
        next: () => {
          this.mensagem = 'Tarefa atualizada com sucesso!';
          this.limparFormulario();
          this.carregarTarefas();
        },
        error: () => {
          this.mensagem = 'Erro ao atualizar tarefa.';
        }
      });
    } else {
      this.taskService.create(this.tarefa).subscribe({
        next: () => {
          this.mensagem = 'Tarefa criada com sucesso!';
          this.limparFormulario();
          this.carregarTarefas();
        },
        error: () => {
          this.mensagem = 'Erro ao criar tarefa.';
        }
      });
    }
  }

  editar(tarefa: TaskItem): void {
    this.editando = true;
    this.idEditando = tarefa.id;

    this.tarefa = {
      titulo: tarefa.titulo,
      descricao: tarefa.descricao,
      status: tarefa.status
    };
  }

  excluir(id: number): void {
    if (!confirm('Deseja excluir esta tarefa?')) {
      return;
    }

    this.taskService.delete(id).subscribe({
      next: () => {
        this.mensagem = 'Tarefa excluída com sucesso!';
        this.carregarTarefas();
      },
      error: () => {
        this.mensagem = 'Erro ao excluir tarefa.';
      }
    });
  }

  limparFormulario(): void {
    this.tarefa = {
      titulo: '',
      descricao: '',
      status: 'Pendente'
    };

    this.editando = false;
    this.idEditando = null;
  }
}