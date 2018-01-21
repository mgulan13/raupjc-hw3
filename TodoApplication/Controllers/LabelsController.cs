using System;
using System.Linq;
using System.Threading.Tasks;
using DataStorage.Interfaces;
using DataStorage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TodoApplication.Models.TodoViewModels;

namespace TodoApplication.Controllers
{

    [Authorize]
    public class LabelsController : Controller
    {
        private readonly ITodoRepository _todoRepository;

        public LabelsController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var labels = await _todoRepository.GetAllLabelsAsync();
            var model = labels.Select(EditLabelViewModel.FromEntity);

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var label = await _todoRepository.GetLabelAsync(id);
            var model = EditLabelViewModel.FromEntity(label);
            
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(EditLabelViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var label = await _todoRepository.GetLabelAsync(model.Id);
            label.Value = model.Value;
            if (!await _todoRepository.UpdateLabelAsync(label))
            {
                ModelState.AddModelError("Value", "Label is already added.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(Guid id)
        {
            await _todoRepository.RemoveLabelAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddLabelViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await _todoRepository.AddLabelAsync(new TodoItemLabel(model.Value)))
            {
                ModelState.AddModelError("Value", "Label is already added.");
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}