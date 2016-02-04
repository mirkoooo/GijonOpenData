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
        string pathOpenData = "http://datos.gijon.es/doc/transporte/busgijontr.xml";
        XNamespace openDataNameSpace = "http://docs.gijon.es/sw/busgijon.asmx";

        /**Realiza una consulta con un número de bus especificado*/
        public void query(string busNumber)
        {
            try
            {
                XDocument document = XDocument.Load(pathOpenData);
                XElement root = document.Root;

                //Guarda toda la información de los buses
                IEnumerable<XElement> busses = root.Element("llegadas").Elements(openDataNameSpace + "llegada");

                //Se realiza una consulta buscando el bus correspondiente
                var query = from XElement element in busses
                            where element.Element(openDataNameSpace + "idlinea").Value.ToString() == busNumber
                            select element;

                //Si se encontraron correspondencais con el número de bus introducido se muestran
                if ((query as IEnumerable<XElement>).Count() > 0)
                {
                    foreach (XElement e in query)
                        Console.WriteLine("Parada: " + e.Element(openDataNameSpace + "idparada").Value +
                            "\tTiempo: " + e.Element(openDataNameSpace + "minutos").Value + "\n");
                }
                else
                    Console.WriteLine("No se encontraron correspondencias con el bus introducido.");
            }

            catch (Exception e)
            {
                Console.WriteLine("Ocurrió un error durante la muestra de datos: " + e.Message);
            }
            
        }
    }
}
