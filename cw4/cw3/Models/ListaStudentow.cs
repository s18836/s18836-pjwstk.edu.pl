using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Models
{
    public class ListaStudentow
    {
        List<Student> lista = new List<Student>();

        public void dodaj(Student st)
        {
            lista.Add(st);
        }

        public List<Student> getlist()
        {
            return lista;
        }
    }
}
