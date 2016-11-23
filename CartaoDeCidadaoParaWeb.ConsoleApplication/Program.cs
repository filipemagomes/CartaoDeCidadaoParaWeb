using Core.Smartcard;
using System;

namespace CartaoDeCidadaoParaWeb.ConsoleApplication
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            using (CardNative card = new CardNative())
            {
                // Obter a lista de leitores.

                string[] leitores =
                    card.ListReaders();


                if (leitores != null && leitores.Length > 0)
                {
                    // Pede ao utilizador para escolher o leitor preferido.

                    bool leitorInvalido = true;

                    int numero = -1;

                    do
                    {
                        Console.WriteLine("Escolha o leitor:");

                        for (int i = 0; i < leitores.Length; i++)
                        {
                            Console.WriteLine(i + "---> " + leitores[i]);
                        }

                        string input = Console.ReadLine();


                        if (!Int32.TryParse(input, out numero) || numero < 0 || numero >= leitores.Length)
                        {
                            Console.WriteLine("O número de leitor é inválido");
                            Console.WriteLine(Environment.NewLine);
                        }
                        else
                        {
                            leitorInvalido = false;
                        }

                    } while (leitorInvalido);

                    Console.WriteLine("Abra agora a página index.html para ver o resultado final");

                    AsyncServer asyncServer = new AsyncServer(leitores[numero]);

                    Console.WriteLine("Prima uma tecla para sair.");
                    Console.Read();

                }
                else
                {
                    Console.WriteLine("Nenhum leitor de cartão de cidadão encontrado.");
                    Console.WriteLine("Prima uma tecla para sair.");
                    Console.Read();
                }

                card.Dispose();
            }
        }
    }
}
