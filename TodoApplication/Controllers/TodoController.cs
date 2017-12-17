﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataStorage.Interfaces;
using DataStorage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

            var todos = new List<TodoViewModel>();
            IndexViewModel model = new IndexViewModel();
            foreach (var todo in _todoRepository.GetActive(userId))
            {
                todos.Add(new TodoViewModel(todo.Id, todo.Text, todo.DateDue, false));
            }
            model.TodoItems = todos;

            return View(model);
        }

        public async Task<IActionResult> Completed()
        {
            var userId = await GetCurrentUserId();

            var todos = new List<TodoViewModel>();
            CompletedViewModel model = new CompletedViewModel();
            foreach (var todo in _todoRepository.GetCompleted(userId))
            {
                todos.Add(new TodoViewModel(todo.Id, todo.Text, todo.DateCompleted, true));
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

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var todoItem = new TodoItem(model.Text, await GetCurrentUserId());
                todoItem.DateDue = model.DateDue;
                if (!String.IsNullOrWhiteSpace(model.Labels))
                {
                    foreach (string labelText in model.Labels.Split(','))
                    {
                        TodoItemLabel label = _todoRepository.AddLabel(new TodoItemLabel(labelText.Trim()));
                        todoItem.Labels.Add(label);
                    }
                }

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