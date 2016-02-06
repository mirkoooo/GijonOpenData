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
    /**Añadida la interfaz, splash screen, los nombres de las paradas, nombre del bus,
    * lista de buses y otras mejoras de aspecto.
    * 06-02-2016
    * Mirko Fañez Kertelj
    * www.mirkoo.es
    */
    class Program
    {
        static Buses bus;

        static string buses;

        static void Main(string[] args)
        {
            bus = new Buses();

            // Splash Screen con mensaje de cargando, para la espera de obtener la lista de buses
            muestraSplashScreen();

            // Obtenemos la lista de los buses disponibles
            buses = bus.obtenerNumerosBuses();

            // Una vez que ya lo tenemos, quitamos la línea de cargando y pedimos una pulsación
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write("  Pulsa una tecla para empezar.");
            Console.ReadLine();

            muestraHUD();
        }

        // Método para mostrar una splash screen, mientras cargan los datos de los buses.
        private static void muestraSplashScreen()
        {
            Console.Clear();
            Console.WriteLine("\n\n"+
                "   .-------------------------------------------------------------.\n" +
                 "  '------..-------------..----------..----------..----------..--.|\n" +
                 "  |       \\\\            ||          ||          ||          ||  ||\n" +
                 "  |        \\\\           ||          ||          ||          ||  ||\n" +
                 "  |    ..   ||  _    _  ||    _   _ || _    _   ||    _    _||  ||\n" +
                 "  |    ||   || //   //  ||   //  // ||//   //   ||   //   //|| /||\n" +
                 "  |_.------\"''----------''----------''----------''----------''--'|\n" +
                 "  .______________________________________________________________|\n" +
                 "   |)|      |       |       |       |    |         |      ||==|  |\n" +
                 "   | |      | _ - _ |       |       |    |  .-.    |      ||==| C|\n" +
                 "   | |  __  |.'.-.' |   _   |   _   |    |.'.-.'.  |  __  | \"__=='\n" +
                 "   '---------'|( )|'----------------------'|( )|'----------\"\"\n" +
                 "               '-'                          '-'\n" +
                 "  --------------------- GIJÓN BUS OPEN DATA ----------------------\n" +
                 "     Han colaborado en este proyecto:\n" +
                 "  \tAndrés Camín\n" +
                 "  \tMirko Fañez\n" + "\n");

            Console.Write("\n  Cargando...");
        }

        // Método para mostrar la interfaz de usuario
        private static void muestraHUD()
        {
            Console.Clear();

            // Mostramos la lista de los buses
            if (!buses.Equals(""))
            {
                Console.WriteLine(String.Format("\n {0,-5} {1}", "BUS", "DESCRIPCIÓN"));
                Console.WriteLine("------------------------------------------------------");

                Console.WriteLine(buses);
            }

            // Preguntamos la línea y hacemos la consulta
            Console.Write("Introduzca la línea de bus a consultar: ");
            bus.query(Console.ReadLine());

            // Preguntamos si quiere consultar otro bus
            Console.Write("\nPulse ESC para salir, o cualquier otra tecla para consultar otra línea.");
            ConsoleKeyInfo cki = Console.ReadKey();

            if (cki.Key != ConsoleKey.Escape)
            {
                // Si no presiona ESC, preguntamos por otro bus
                muestraHUD();
            }
        }
    }
}
