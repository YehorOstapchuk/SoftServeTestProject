using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.BL;
using TodoApi.DAL;
using TodoApi.DataRepository;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Tests
{
    public class TodoTestBL
    {
        private Mock<IEntityRepository<TodoItem>> todoRepository;
        private List<TodoItem> todoItems;
        [SetUp]
        public void Setup()
        {
            //Set up the mock
            todoRepository = new Mock<IEntityRepository<TodoItem>>();
            todoItems = new List<TodoItem>();
            todoItems.Add(new TodoItem() { Id = 0, Name = "todo0", IsComplete = false, Secret = "0" });
            todoItems.Add(new TodoItem() { Id = 1, Name = "todo1", IsComplete = false, Secret = "1" });
            todoItems.Add(new TodoItem() { Id = 2, Name = "todo2", IsComplete = false, Secret = "2" });
        }

        [Test]
        public async Task TestGetAll()
        {
            //Act
            todoRepository.Setup(x => x.GetAllEnumerable()).Returns(todoItems.AsEnumerable<TodoItem>());
            
            //Arrange
            var todoBL = new TodoBL(todoRepository.Object);
            var todoList = await todoBL.GetAll();
       
            //Assert
            Assert.AreEqual(todoList.Count(), 3);
        }

        [Test]
        public async Task TestGetItemById()
        {
            //Act
            todoRepository.Setup(a => a.GetAllEnumerable()).Returns(todoItems.AsEnumerable<TodoItem>());

            //Arrange
            var todoBL = new TodoBL(todoRepository.Object);
            var todoItem = await todoBL.Find(1);

            //Assert
            Assert.AreEqual(todoItem.Id, 1);
            Assert.AreEqual(todoItem.Name, "todo1");
        }

        [Test]
        public async Task UpdateTodoItem()
        {
            //Act
            todoRepository.Setup(a => a.GetAllEnumerable()).Returns(todoItems.AsEnumerable<TodoItem>());
            //Arrange
            var todoBL = new TodoBL(todoRepository.Object);
            TodoItemDTO item = new TodoItemDTO { Id = 2, Name = "todo22", IsComplete = true };
            await todoBL.Update(2, item);
            TodoItem todoItem = todoItems.Where(a => a.Id == 2).First();
            TodoItem temp = await todoBL.Find(2);
            //Assert
            Assert.AreEqual(temp.Name, "todo22");
        }

        [Test]
        public async Task DeleteTodoItem()
        {
            //Act
            todoRepository.Setup(a => a.GetAllEnumerable()).Returns(todoItems.AsEnumerable<TodoItem>());
            //Arrange
            var todoBL = new TodoBL(todoRepository.Object);
            await todoBL.Delete(1);
            //Assert
            todoRepository.Verify(x => x.Remove(It.IsAny<TodoItem>()));
        }

        [Test]
        public async Task CreateTodoItem()
        {
            //Act
            todoRepository.Setup(a => a.GetAllEnumerable()).Returns(todoItems.AsEnumerable<TodoItem>());
            //Arrange
            var todoBL = new TodoBL(todoRepository.Object);
            TodoItemDTO temp = new TodoItemDTO { Name = "temp0", IsComplete = false };
            await todoBL.Create(temp);
            //Assert
            todoRepository.Verify(x => x.Insert(It.IsAny<TodoItem>()));
        }
    }
}
