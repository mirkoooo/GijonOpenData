using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Prueba_XMLOpenData
{
    class Buses
    {
        const string pathOpenData = "http://datos.gijon.es/doc/transporte/busgijontr.xml";
        XNamespace openDataNameSpace = "http://docs.gijon.es/sw/busgijon.asmx";
        const string pathBusInfo = "http://datos.gijon.es/doc/transporte/busgijoninfo.xml"; // XML con los nombres de paradas

        List<string> idsParadas;
        List<int> minsParadas;
        Dictionary<string,string> nombresParadas; // Diccionario con idParada, nombre

        /**Realiza una consulta con un número de bus especificado*/
        public void query(string busNumber)
        {
            try
            {
                // Inicializamos las listas
                idsParadas = new List<string>();
                minsParadas = new List<int>();
                nombresParadas = new Dictionary<string, string>();

                XDocument document = XDocument.Load(pathOpenData);
                XElement root = document.Root;

                //Guarda toda la información de los buses
                IEnumerable<XElement> buses = root.Element("llegadas").Elements(openDataNameSpace + "llegada");

                // Se realiza una consulta buscando el bus correspondiente.
                // Ordenado por los minutos que le faltan
                var query = from XElement element in buses
                            where element.Element(openDataNameSpace + "idlinea").Value.ToString() == busNumber
                            orderby Convert.ToInt32(element.Element(openDataNameSpace + "minutos").Value)
                            select element;

                //Si se encontraron correspondencias con el número de bus introducido se muestran
                if (query.Count() > 0)
                {
                    // Mostramos una cabecera con el nombre del bus
                    cabeceraInfo(busNumber);
                    
                    foreach (XElement e in query)
                    {
                        // Obtenemos los datos
                        string idParada = e.Element(openDataNameSpace + "idparada").Value;
                        int minutos = Convert.ToInt32(e.Element(openDataNameSpace + "minutos").Value);

                        // Los añadimos a las listas
                        idsParadas.Add(idParada);
                        minsParadas.Add(minutos);
                    }

                    // Obtenemos los nombres de todas las paradas encontradas
                    obtenerNombresParadas(idsParadas);

                    // Lo mostramos todo
                    Console.WriteLine(String.Format(" {0,-7}  {1}", "MINUTOS", "PARADA"));
                    Console.WriteLine("----------------------------------------");

                    for(int i = 0; i < idsParadas.Count; i++)
                    {
                        // Obtenemos los datos de las paradas
                        string id = idsParadas[i];
                        string nombre;
                        nombresParadas.TryGetValue(id, out nombre);

                        if (nombre != null)
                        {
                            Console.WriteLine(String.Format(" {0,-7}  {1} ({2})",
                                minsParadas[i].ToString("00"), nombre, id));
                        }
                    }
                }
                else
                    Console.WriteLine("\nNo se encontraron correspondencias con el bus introducido.");
            }

            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("\n Ocurrió un error durante la muestra de datos:\n" + e.Message);
            }
            
        }

        // Método para obtener el nombre de todas las paradas dentro de una lista.
        // Lo hacemos así por eficiencia a la hora de acceder a un recurso de internet.
        private void obtenerNombresParadas(List<string> paradas)
        {
            try
            {
                XDocument document = XDocument.Load(pathBusInfo);
                XElement root = document.Root;

                // Guarda toda la información de las paradas
                IEnumerable<XElement> paradasInfo = root.Element("paradas").Elements(openDataNameSpace + "parada");

                //Se realiza una consulta buscando todas las paradas que tenemos.
                // Obtenemos todos los nombres
                var query = from XElement element in paradasInfo
                            where paradas.Contains(element.Element(openDataNameSpace + "idparada").Value.ToString())
                            select new { ID = element.Element(openDataNameSpace + "idparada").Value, NOMBRE = element.Element(openDataNameSpace + "descripcion").Value };

                // Si pudimos obtener la información, añadimos los dos campos al diccionario
                if (paradasInfo.Count() > 0)
                {
                    foreach(var parada in query)
                    {
                        // Lo añadimos al diccionario
                        nombresParadas.Add(parada.ID, parada.NOMBRE);
                    }
                }
            }
            catch (Exception)
            {
                // No hacemos nada
            }
        }

        private void cabeceraInfo(string busNumber)
        {
            // Mostramos un pequeño bus
            Console.Clear();

            // Lo convertimos a número para mostrarlo con 2 caracteres
            int numeroBus = Convert.ToInt32(busNumber);

            Console.WriteLine("\n        _____________\n" +
                                "      _/_|[][][][][] | - -\n" +
                                "     (     Bus " + numeroBus.ToString("00") + "    | - -\n" +
                                "     =--OO-------OO--=\n");
            Console.WriteLine(" Tiempo para las llegadas del bus:\n");
        }

        // Método que devuelve los números de los buses junto con su descripción
        public string obtenerNumerosBuses()
        {
            string buses = "";

            try
            {
                XDocument document = XDocument.Load(pathBusInfo);
                XElement root = document.Root;

                // Guarda toda la información de las líneas. El namespace es el mismo
                IEnumerable<XElement> busInfo = root.Element("lineas").Elements(openDataNameSpace + "linea");

                //Si pudimos obtener la información, obtenemos el número y nombre de la línea
                if (busInfo.Count() > 0)
                {
                    foreach (XElement e in busInfo)
                    {
                        string numLinea = e.Element(openDataNameSpace + "idlinea").Value;
                        string nombre = e.Element(openDataNameSpace + "descripcion").Value;

                        // Formateamos la salida
                        buses += String.Format(" {0,-5} {1}\n", numLinea, nombre);
                    }
                }
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("\n\nNo se pueden obtener los datos de los autobuses.");
                Console.WriteLine("Pulse una tecla para salir.");
                Console.ReadLine();

                // Cerramos la aplicación
                Environment.Exit(0);
            }

            return buses;
        }
    }
}
