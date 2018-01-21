using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataStorage.Interfaces;
using DataStorage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TodoApplication.Models;
using TodoApplication.Models.TodoViewModels;


namespace TodoApplication.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _todoRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoRepository todoRepository, UserManager<ApplicationUser> userManager)
        {
            _todoRepository = todoRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = await GetCurrentUserId();

            var todoItems = new List<TodoViewModel>();
            IndexViewModel model = new IndexViewModel();
            foreach (var todo in _todoRepository.GetActive(userId))
            {
                todoItems.Add(new TodoViewModel()
                {
                    Id = todo.Id,
                    Text = todo.Text,
                    Date = todo.DateDue,
                    IsCompleted = false
                });
            }
            model.TodoItems = todoItems;

            return View(model);
        }

        public async Task<IActionResult> Completed()
        {
            var userId = await GetCurrentUserId();

            var todos = new List<TodoViewModel>();
            CompletedViewModel model = new CompletedViewModel();
            foreach (var todo in _todoRepository.GetCompleted(userId))
            {
                todos.Add(new TodoViewModel()
                {
                    Id = todo.Id,
                    Text = todo.Text,
                    Date = todo.DateCompleted,
                    IsCompleted = true
                });
            }
            model.TodoItems = todos;

            return View(model);
        }

        public async Task<IActionResult> MarkAsCompleted(Guid todoId)
        {
            var userId = await GetCurrentUserId();
            _todoRepository.MarkAsCompleted(todoId, userId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(Guid todoId)
        {
            var userId = await GetCurrentUserId();
            _todoRepository.Remove(todoId, userId);
            return RedirectToAction("Completed");
        }

        public async  Task<IActionResult> Add()
        {
            ViewBag.Labels = new MultiSelectList(await _todoRepository.GetAllLabelsAsync(), "Id", "Value");
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel model)

        {
            if (ModelState.IsValid)
            {
                var labels = new List<TodoItemLabel>();
                if (model.Labels != null)
                {
                    foreach (var label in model.Labels)
                    {
                        labels.Add(await _todoRepository.GetLabelAsync(label));
                    }
                }
                var todoItem = new TodoItem(model.Text, await GetCurrentUserId())
                {
                    DateDue = model.DateDue,
                    Labels = labels
                };

                _todoRepository.Add(todoItem);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        private async Task<Guid> GetCurrentUserId()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return new Guid(user.Id);
        }
    }
}