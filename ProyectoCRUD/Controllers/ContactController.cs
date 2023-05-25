using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using ProyectoCRUD.Models;
using System.Data.SqlClient;
using System.Data;

namespace ProyectoCRUD.Controllers
{
    public class ContactController : Controller
    {
       
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString(); 

        private static List<Contacto> oLista=new List<Contacto>();
        
        public ActionResult Inicio()
        {
            oLista = new List<Contacto>();

            

            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                
                SqlCommand cmd = new SqlCommand("select * from dbo.Contacto", oConexion);
               

                cmd.CommandType = CommandType.Text;
                
                oConexion.Open();
                
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Contacto nuevoContacto = new Contacto();
                       
                        nuevoContacto.IdContacto = Convert.ToInt32(dr["IdContacto"]);
                        nuevoContacto.Nombres = dr["Nombres"].ToString();
                        nuevoContacto.Apellidos = dr["Apellidos"].ToString();
                        nuevoContacto.Telefono = dr["Telefono"].ToString();
                        nuevoContacto.Correo = dr["Correo"].ToString();

                        
                        oLista.Add(nuevoContacto);
                    }
                }
            }
            
                return View(oLista);
        }

        public ActionResult Registrar() {


            return View();
        }


       
        [HttpGet]
        public ActionResult Editar(int? idcontacto)
        {
            if (idcontacto == null)
                return RedirectToAction("Inicio", "Contact");
            
           
             Contacto ocontacto = oLista.Where(c => c.IdContacto == idcontacto).FirstOrDefault();
            
            return View(ocontacto);
        }

        [HttpGet]
        public ActionResult RegistrarContacto() {
            return View();
        }

       
        [HttpPost]
        public ActionResult RegistrarContacto(Contacto oContacto) {

            using (SqlConnection oconexion = new SqlConnection(conexion)) {
                SqlCommand cmd = new SqlCommand("sp_Registrar",oconexion);
                cmd.Parameters.AddWithValue("Nombres", oContacto.Nombres);
                cmd.Parameters.AddWithValue("Apellidos",oContacto.Apellidos);
                cmd.Parameters.AddWithValue("Telefono", oContacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", oContacto.Correo);
                
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio","Contact");
        }
       

        [HttpPost]
        public ActionResult Editar(Contacto oContacto)
        {

            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Editar", oconexion);
                cmd.Parameters.AddWithValue("IdContacto", oContacto.IdContacto);
                cmd.Parameters.AddWithValue("Nombres", oContacto.Nombres);
                cmd.Parameters.AddWithValue("Apellidos", oContacto.Apellidos);
                cmd.Parameters.AddWithValue("Telefono", oContacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", oContacto.Correo);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Inicio","Contact");

        }
    }
}