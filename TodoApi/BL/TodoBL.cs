using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.DAL;
using TodoApi.DataRepository;

namespace TodoApi.BL
{
    public class TodoBL
    {
        private IEntityRepository<TodoItem> _repo;

        public TodoBL(TodoContext context)
        {
            _repo = new EntityRepository<TodoItem>(context);
        }

        public TodoBL(IEntityRepository<TodoItem> repository)
        {
            _repo = repository;
        }

        public async Task<IEnumerable<TodoItem>> GetAll()
        {
            return await Task.FromResult( _repo.GetAllEnumerable().ToList());
        }

        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetAllDTO()
        {
            return await Task.FromResult(_repo.GetAllEnumerable().Select(x => ItemToDTO(x)).ToList());
        }

        public async Task<TodoItem> Find(long id)
        {
            return await Task.FromResult ( _repo.GetAllEnumerable().Where(x => x.Id == id).FirstOrDefault());
        }

        public async Task<TodoItemDTO> FindDTO(long id)
        {
            return await Task.FromResult( _repo.GetAllEnumerable().Select(x => ItemToDTO(x)).Where(x => x.Id == id).FirstOrDefault());
        }


        public async Task<int> Update(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id) throw new ArgumentException("Wrong Id");
            TodoItem temp = await Find(id);
            if (temp == null) throw new ArgumentException("Not Found");
            return await Update(temp, todoItemDTO);
        }

        public async Task<int> Update (TodoItem oldItem, TodoItem newItem )
        {
            oldItem.Name = newItem.Name;
            oldItem.IsComplete = newItem.IsComplete;
            return await _repo.SaveAsync();
        }

        public async Task<int> Update(TodoItem oldItem, TodoItemDTO newItem)
        {
            oldItem.Name = newItem.Name;
            oldItem.IsComplete = newItem.IsComplete;
            return await _repo.SaveAsync();
        }

        public async Task<TodoItem> Create (TodoItemDTO item)
        {
            var itemResult = new TodoItem { Name = item.Name, IsComplete = item.IsComplete };
            _repo.Insert(itemResult);
            await _repo.SaveAsync();
            return itemResult;
        }

        public async Task<int> Delete(long id)
        {
            TodoItem temp = await Find(id);
            if (temp == null) throw new ArgumentException("Not Found");
            _repo.Remove(temp);
            return await _repo.SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _repo.SaveAsync();
        }

        public bool Exists(long id)
        {
            return _repo.GetAllEnumerable().Any(x => x.Id == id);
        }

        public static IEnumerable<TodoItemDTO> ItemListToDtoList(IEnumerable<TodoItem> list)
        {
            return list.Select(x => ItemToDTO(x)).ToList<TodoItemDTO>();
        }

        public static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
                                                                 new TodoItemDTO
                                                                 {
                                                                     Id = todoItem.Id,
                                                                     Name = todoItem.Name,
                                                                     IsComplete = todoItem.IsComplete
                                                                 };

    }
}
