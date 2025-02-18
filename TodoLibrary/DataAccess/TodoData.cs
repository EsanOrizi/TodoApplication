﻿using TodoLibrary.Models;

namespace TodoLibrary.DataAccess
{
    public class TodoData : ITodoData
    {
        private readonly ISqlDataAccess sql;

        public TodoData(ISqlDataAccess sql)
        {
            this.sql = sql;
        }

        public Task<List<TodoModel>> GetAllAssigned(int assignedTo)
        {
            return sql.LoadData<TodoModel, dynamic>("spTodos_GetAllAssigned",
                new { AssignedTo = assignedTo }, "Default");
        }


        public async Task<TodoModel?> GetOneAssigned(int assignedTo)
        {
            var results = await sql.LoadData<TodoModel, dynamic>("spTodos_GetOneAssigned",
                new { AssignedTo = assignedTo }, "Default");

            return results.FirstOrDefault();
        }


        public async Task<TodoModel?> Create(int assignedTo, string task)
        {
            var results = await sql.LoadData<TodoModel, dynamic>("spTodos_Create",
                new { AssignedTo = assignedTo, Task = task }, "Default");

            return results.FirstOrDefault();
        }

        public Task UpdateTask(int assignedTo, int todoId, string task)
        {
            return sql.SaveData<dynamic>("spTodos_UpdateTask",
                new { AssignedTo = assignedTo, TodoId = todoId, Task = task }, "Default");
        }


        public Task CompleteTodo(int assignedTo, int todoId)
        {
            return sql.SaveData<dynamic>("spTodos_CompeleteTodo",
                new { AssignedTo = assignedTo, TodoId = todoId }, "Default");
        }


        public Task Delete(int assignedTo, int todoId)
        {
            return sql.SaveData<dynamic>("spTodos_Delete",
                new { AssignedTo = assignedTo, TodoId = todoId }, "Default");
        }

    }
}
