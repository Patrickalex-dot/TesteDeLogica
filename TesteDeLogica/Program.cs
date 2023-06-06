using System;

namespace TesteDeLogica
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Cria Bolinhas
            Network network = new Network(8);

            //Cria Conexões
            network.Connect(1, 2);
            network.Connect(6, 2);
            network.Connect(2, 4);
            network.Connect(5, 8);


            //Valida Conexões
             Console.WriteLine(network.Query(1, 6)); // True
             Console.WriteLine(network.Query(6, 4)); // True
             Console.WriteLine(network.Query(7, 4)); // False
             Console.WriteLine(network.Query(5, 6)); // False
             

        }
    }
    
}
