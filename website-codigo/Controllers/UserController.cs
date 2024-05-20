using Hal.Data;
using Hal.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hal.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _dataContext;
        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // ### PAGINA LOGIN
        public IActionResult LoginPage()
        {
            return View();
        }

        // Efetuar Login
        public IActionResult EfetuarLogin(LoginDTO request)
        {
            var session = HttpContext.Session.GetString("_Id");
            var find = _dataContext.Users.Find(session);
            if (find != null)
            {
                return RedirectToAction("PerfilPage", "User");
            }

            var getUser = _dataContext.Users.FirstOrDefault(x => x.UserEmail == request.Email);
            if (getUser == null || !BCrypt.Net.BCrypt.Verify(request.Password, getUser.PasswordHash))
            {
                TempData["ErrorMessage"] = "Usuário ou senha incorretos.";
                return RedirectToAction("LoginPage");
            }

            if (getUser.IsActive == false)
            {
                getUser.IsActive = true;
            }

            HttpContext.Session.SetInt32("_Id", getUser.Id);
            HttpContext.Session.SetString("_Email", getUser.UserEmail);

            return RedirectToAction("Index", "Home");
        }


        // Efetuar o LogOut
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginPage");
        }

        // ### PAGINA REGISTER
        public IActionResult RegisterPage()
        {
            var session = HttpContext.Session.GetString("_Id");
            var find = _dataContext.Users.Find(session);
            if (find != null)
            {

                return RedirectToAction("PerfilPage", "User");
            }

            return View();
        }

        // TODO - Obter IFRAME com dashboard relativo ao usuário

        // ### PAGINA PERFIL DO USUARIO
        public IActionResult PerfilPage()
        {
            var id = HttpContext.Session.GetInt32("_Id");
            if (id == null)
            {
                return RedirectToAction("LoginPage");
            }

            var getUser = _dataContext.Users.Find(id);
            if (getUser == null)
            {
                return NotFound();
            }

            ViewBag.User = getUser;

            // Buscar comentários atrelados ao UserId
            var comments = _dataContext.Comments
                .Where(c => c.UserId == id)
                .Include(c => c.Category)
                .Include(c => c.Evaluation)
                .OrderByDescending(c => c.Id)
                .ToList();

            ViewBag.Comments = comments;
            return View();
        }

        // Método para adicionar User
        public IActionResult EfetuarCadastro(CadastroDTO request) 
        {
            var findUser = _dataContext.Users.FirstOrDefault(x => x.UserEmail == request.UserEmail);
            if (findUser != null)
            {
                //TODO: Retornar um card com a informaç~eo que há cadastro
                return NotFound();
            }

            User newUser = new User
            {
                UserEmail = request.UserEmail,
                UserName = request.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash),
                DateRegister = DateTime.Now,
            };

            _dataContext.Users.Add(newUser);
            _dataContext.SaveChanges();

            return RedirectToAction("LoginPage", "User");
        }

        // Método para editar User
        public IActionResult EditarPerfil(CadastroDTO request)
        {
            var id = HttpContext.Session.GetInt32("_Id");
            if (id == null)
            {
                TempData["ErrorMessage"] = "Usuário não encontrado.";
                return RedirectToAction("PerfilPage", new { activeTab = "dados" });
            }

            var getUser = _dataContext.Users.Find(id);
            if (getUser == null)
            {
                TempData["ErrorMessage"] = "Usuário não encontrado.";
                return RedirectToAction("PerfilPage", new { activeTab = "dados" });
            }

            if (ModelState.IsValid)
            {
                getUser.UserEmail = request.UserEmail;
                getUser.UserName = request.UserName;
                if (!string.IsNullOrEmpty(request.PasswordHash))
                {
                    getUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
                }

                _dataContext.Update(getUser);
                _dataContext.SaveChanges();
                TempData["SuccessMessage"] = "Perfil atualizado com sucesso.";
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao atualizar o perfil.";
            }

            return RedirectToAction("PerfilPage", new { activeTab = "dados" });
        }


        // Método para deletar User
        public IActionResult DeletarPerfil(int id)
        {
            var getUser = _dataContext.Users.Find(id);
            getUser.IsActive = false;

            _dataContext.Update(getUser);
            _dataContext.SaveChanges();

            return RedirectToAction("PerfilPage");
        }


        // ### Commments

        // ### Criar Comentário
        [HttpPost]
        public IActionResult PostarComentario(CommentDTO request)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    UserId = request.UserId,
                    EvaluationId = request.EvaluationId != 0 ? request.EvaluationId : null, // Permitir nulo se não fornecido
                    CategoryId = request.CategoryId != 0 ? request.CategoryId : null, // Permitir nulo se não fornecido
                    CommentText = request.CommentText,
                    CommentDate = DateTime.Now
                };

                _dataContext.Comments.Add(comment);
                _dataContext.SaveChanges();

                TempData["SuccessMessage"] = "Comentário postado com sucesso.";
                return RedirectToAction("PerfilPage", new { activeTab = "comentarios" });
            }

            TempData["ErrorMessage"] = "Ocorreu um erro ao postar o comentário.";
            return RedirectToAction("PerfilPage", new { activeTab = "comentarios" });
        }

        // ### Editar Comentário
        [HttpPost]
        public IActionResult EditarComentario(int id, CommentDTO request)
        {
            var comment = _dataContext.Comments.FirstOrDefault(c => c.Id == id);
            if (comment != null && ModelState.IsValid)
            {
                // Verificar se o EvaluationId é válido e existe
                if (request.EvaluationId != null && request.EvaluationId != 0)
                {
                    var evaluationExists = _dataContext.Evaluations.Any(e => e.Id == request.EvaluationId);
                    if (!evaluationExists)
                    {
                        TempData["ErrorMessage"] = "A avaliação especificada não existe.";
                        return RedirectToAction("PerfilPage", new { activeTab = "comentarios" });
                    }
                }
                else
                {
                    request.EvaluationId = 1; // Usar ID padrão se não for fornecido
                }

                // Verificar se o CategoryId é válido e existe
                if (request.CategoryId != null && request.CategoryId != 0)
                {
                    var categoryExists = _dataContext.Categories.Any(c => c.Id == request.CategoryId);
                    if (!categoryExists)
                    {
                        TempData["ErrorMessage"] = "A categoria especificada não existe.";
                        return RedirectToAction("PerfilPage", new { activeTab = "comentarios" });
                    }
                }
                else
                {
                    request.CategoryId = 1; // Usar ID padrão se não for fornecido
                }

                comment.CommentText = request.CommentText;
                comment.EvaluationId = request.EvaluationId;
                comment.CategoryId = request.CategoryId;
                comment.CommentDate = DateTime.Now;
                _dataContext.SaveChanges();
                TempData["SuccessMessage"] = "Comentário editado com sucesso.";
                return RedirectToAction("PerfilPage", new { activeTab = "comentarios" });
            }

            TempData["ErrorMessage"] = "Ocorreu um erro ao editar o comentário.";
            return RedirectToAction("PerfilPage", new { activeTab = "comentarios" });
        }


        // ### Deletar Comentário
        [HttpPost]
        public IActionResult DeletarComentario(int id)
        {
            var comment = _dataContext.Comments.FirstOrDefault(c => c.Id == id);
            if (comment != null)
            {
                _dataContext.Comments.Remove(comment);
                _dataContext.SaveChanges();
                TempData["SuccessMessage"] = "Comentário deletado com sucesso.";
            }
            else
            {
                TempData["ErrorMessage"] = "Comentário não encontrado.";
            }
            return RedirectToAction("PerfilPage", new { activeTab = "comentarios" });
        }
    }
}
