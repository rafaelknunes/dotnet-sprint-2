using Hal.Data;
using Hal.DTOs;
using Hal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;


namespace Hal.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly DataContext _dataContext;

        public AdminController(ILogger<AdminController> logger, DataContext dataContext)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        // ## Página Principal
        // Método para visualizar categorias ou comentários
        public IActionResult Index()
        {
            var categories = _dataContext.Categories
                .OrderByDescending(c => c.Id)
                .ToList();

            var comments = _dataContext.Comments
                .Include(c => c.User)
                .Include(c => c.Category)
                .Include(c => c.Evaluation)
                .OrderByDescending(c => c.Id)
                .ToList();

            var random = new Random();
            var productNames = new[] {
                "Conta Corrente Premium",
                "Investimento Ouro",
                "Plano de Previdência Privada",
                "Seguro de Vida Platinum",
                "Fundo Imobiliário Prime",
                "Consórcio de Veículos",
                "Título de Capitalização",
                "Cartão de Crédito Black",
                "Empréstimo Consignado",
                "Plano de Saúde Plus"
            };
            var listaDeInfos = comments.Select(c => new {
                Comment = c,
                Percentage = random.Next(5, 71),
                ProductName = productNames[random.Next(productNames.Length)]
            }).ToList();

            ViewBag.Comments = listaDeInfos;

            return View(categories);
        }

        // ### COMENTARIOS


        // ### CATEGORIAS


        // Método para criar categoria (POST)
        [HttpPost]
        public IActionResult CriarCategoria(CategoriaDTO request)
        {
            if (ModelState.IsValid)
            {
                var categoria = new Category
                {
                    // Não definir o Id aqui, deixar o banco de dados gerar automaticamente
                    CategoryName = request.CategoryName,
                    SubCategoryName = request.SubCategoryName
                };

                _dataContext.Categories.Add(categoria);
                _dataContext.SaveChanges();

                // Log the details of the category
                _logger.LogInformation("###### Categoria criada com ID: {Id}, Nome: {CategoryName}, SubCategoria: {SubCategoryName}",
                                       categoria.Id, categoria.CategoryName, categoria.SubCategoryName);


                TempData["SuccessMessage"] = "Categoria criada com sucesso.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Erro ao criar a categoria.";
            return RedirectToAction("Index");
        }

        // Método para editar categoria (POST)
        [HttpPost]
        public IActionResult EditarCategoria(int id, CategoriaDTO request)
        {
            var category = _dataContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null && ModelState.IsValid)
            {
                category.CategoryName = request.CategoryName;
                category.SubCategoryName = request.SubCategoryName;
                _dataContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(request);
        }

        // Método para deletar categoria (POST)
        [HttpPost]
        public IActionResult DeletarCategoria(int id)
        {
            var category = _dataContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _dataContext.Categories.Remove(category);
                _dataContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
