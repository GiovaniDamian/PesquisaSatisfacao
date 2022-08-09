using Microsoft.AspNetCore.Mvc;
using PesquisaSatisfacao.Models;
using System.Diagnostics;

namespace PesquisaSatisfacao.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public ActionResult PesquisaSatisfacao()
        {
            ViewData["usuario"] = this.Usuario.Name;
            ViewData["data"] = DateTime.Now;

            var duplicacao = PesquisaModel.EncontrarPesquisaDuplicada(this.Usuario.Name);
            var datamodal = PesquisaModel.PesquisarDataModal(this.Usuario.Name);

            if (!duplicacao)
            {
                var dataPesquisa = Convert.ToDateTime(datamodal);
                if (DateTime.Now.DayOfYear >= dataPesquisa.DayOfYear && DateTime.Now.TimeOfDay >= dataPesquisa.TimeOfDay)
                {
                    PesquisaModel.AtualizarDataModal(this.Usuario.Name, DateTime.Now.AddDays(1));
                    return PartialView("_PV_PesquisaSatisfacao", ViewData);

                }
            }
            return new EmptyResult();
        }

        [HttpPost]
        public bool IncluirPesquisaNoBanco(PesquisaModel form)
        {
            var sucesso = PesquisaModel.Incluir(form.Texto, form.Nota, form.Usuario, form.DataRealizada);

            return sucesso;
        }


        public ActionResult IncluirHorarioModal()
        {
            var usuario = this.Usuario.Name;
            var dataModal = DateTime.Now;
            var horariomodal = PesquisaModel.PesquisarDataModal(this.Usuario.Name);

            if (horariomodal == "False")
            {
                PesquisaModel.IncluirDataModal(usuario, dataModal);
            }

            return new EmptyResult();
        }
    }
}