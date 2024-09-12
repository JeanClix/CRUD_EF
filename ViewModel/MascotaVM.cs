using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TAREA___CRUD_CON_EF.Models;

namespace TAREA___CRUD_CON_EF.ViewModel
{
public class MascotaVM
    {
        public Mascota FormMascota { get; set; }

        [ValidateNever]
        public List<Mascota> ListMascota { get; set; }

        public MascotaVM()
        {
            FormMascota = new Mascota();
            ListMascota = new List<Mascota>();
        }
    }
}