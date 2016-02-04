using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_XMLOpenData
{
    /**Programa para la consulta de las líneas de buses de Emtusa de Gijón
     * Información obtenida del Portal de Transparencia del Ayuntamiento de Gijón
     * https://transparencia.gijon.es/
     * 21-01-2016
     * Andrés Camín Fernández
     */
    class Program
    {
        static void Main(string[] args)
        {
            Buses bus = new Buses();

            Console.WriteLine("Introduce la linea de bus a consultar:");
            bus.query(Console.ReadLine());

            Console.Write("Pulse una tecla para continuar...");
            Console.ReadKey();
        }
    }
}
