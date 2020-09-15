using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProyClase3.Models;

namespace ProyClase3.Controllers
{
    public class AlumnosController : Controller
    {

        static List<Alumno> Lista= new List<Alumno>()
        {
            new Alumno(){Legajo=100, Nombre="Stella", Apellido="Lopez", PaisNombre="Argentina",Sexo="Femenino",Dni=35928121 },
            new Alumno(){Legajo=23, Nombre="Virginia", Apellido="Martinez", PaisNombre="Argentina",Sexo="Femenino",Dni=36209325 },
            new Alumno(){Legajo=15, Nombre="Rodolfo", Apellido="Rafen", PaisNombre="Argentina",Sexo="Masculino",Dni=39441227 },
            new Alumno(){Legajo=103, Nombre="Guillermo", Apellido="Tanderi", PaisNombre="Bolivia",Sexo="Masculino",Dni=32452958 },
        };
        public List<Pais> Paises=new List<Pais>(){
            new Pais(){Id=1,Nombre="Argentina"},
            new Pais(){Id=2,Nombre="Bolivia"},
            new Pais(){Id=3,Nombre="Uruguay"},
            new Pais(){Id=4,Nombre="Paraguay"},
            new Pais(){Id=5,Nombre="Chile"},
            //prueba
            new Pais(){Id=6,Nombre="Brasil"},
        };
        public IActionResult Index()
        {
            //consulto BD
            ViewBag.MensajeBienvenida="Gestion de Alumnos";                
            Lista=Lista.OrderBy(s=>s.Legajo).ToList();
            return View(Lista);
        }

        
        public IActionResult Order()
        {
            //consulto BD
            ViewBag.MensajeBienvenida="Estamos probando el viewbag"; 
            Lista=Lista.OrderByDescending(s=>s.Legajo).ToList();
            return View(Lista);
        }





        public IActionResult Editar(int leg){
            ViewBag.Paises=Paises;
            var alumnoDB= Lista.Where(s=> s.Legajo==leg).FirstOrDefault();
            return View(alumnoDB);
        }

        [HttpPost]
        public IActionResult Editar(Alumno alumnoFormulario,IFormCollection formulario){
          
            var alumnoAnterior= Lista.Where(s => s.Legajo == alumnoFormulario.Legajo).FirstOrDefault();
            var paisSeleccionado=Paises.Where(x => x.Id == int.Parse(alumnoFormulario.PaisNombre) ).FirstOrDefault();
            alumnoFormulario.PaisNombre=paisSeleccionado.Nombre;
            alumnoFormulario.Sexo=formulario["genero"];
            Lista.Remove(alumnoAnterior);
            Lista.Add(alumnoFormulario);           
            return RedirectToAction("Index");
        }

        public IActionResult Crear()
        {
            ViewBag.Paises=Paises;
            return View();
        }

      

        [HttpPost, ActionName("Crear")]
        public IActionResult GuardarNuevoAlumno(Alumno alumnoFormulario,IFormCollection formulario){
            //validacion legajo existente
            var alumnoDB= Lista.Where(s=> s.Legajo==alumnoFormulario.Legajo).FirstOrDefault();

            var alumnoAnterior= Lista.Where(s => s.Legajo == alumnoFormulario.Legajo).FirstOrDefault();
            var paisSeleccionado=Paises.Where(x => x.Id == int.Parse(alumnoFormulario.PaisNombre) ).FirstOrDefault();
            alumnoFormulario.PaisNombre=paisSeleccionado.Nombre;
            alumnoFormulario.Sexo=formulario["genero"];
            
            if(alumnoDB!=null)
            {return Content("ERROR, LEGAJO EXISTENTE");}
            else
            {Lista.Add(alumnoFormulario);return RedirectToAction("Index"); }
            
        }

        public IActionResult Detalle(int leg){
            ViewBag.Paises=Paises;
            var alumnoDB= Lista.Where(s=> s.Legajo==leg).FirstOrDefault();
            
            return View(alumnoDB);
        }

        [HttpPost,ActionName("Borrar")]
        public IActionResult Borrar(Alumno alumnoFormulario){
            var alumnoDB= Lista.Where(s=> s.Legajo==alumnoFormulario.Legajo).FirstOrDefault();
            Lista.Remove(alumnoDB);
            return Content("BORRADO");
        }

    }
}
