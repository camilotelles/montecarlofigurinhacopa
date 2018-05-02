using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// um pacote custa R$ 2,00
// um pacote vem 5 figurinhas 
// não existe diferença na distribuição das figurinhas
// não existem figurinhas repetidas em um pacote

namespace MonteCarloFigurinhasCopa
{
    class Result
    {
        public int numberofPackages;
        public int[] used;

        public Result()
        {
            used = new int[4000]; //ugly, we know
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Random rnd1 = new Random();

            bool[] stickerAlbum = new bool[682];
            List<int> stickerPackage = new List<int>();
            List<Result> resultList = new List<Result>();
            int numberofPackages;
            const int numberStickersPackage = 5;

            for (int run = 0; run < 10000; run++) // vou rodar a simulação 10.000 vezes
            {
                Result result = new Result();
                for (int tmp = 0; tmp < 682; tmp++)
                    stickerAlbum[tmp] = false;
                int numberStickersUnfound= 682;
                int stickerTmp = 0; // qual a figurinha que vamos adicionar no pacote. 
                numberofPackages = 0;
                int numberofStickersUsed = 0;
                
                while (numberStickersUnfound > 0) // enquanto ainda tiver faltando figurinhas no album
                {
                    // fill a package
                    
                    while (stickerPackage.Count < numberStickersPackage)
                    {
                        stickerTmp = rnd1.Next(682);
                        if (!stickerPackage.Exists(element => element == stickerTmp))
                            stickerPackage.Add(stickerTmp);
                    }
                    // one more package
                    numberofPackages++;
                    numberofStickersUsed = 0; // numero de figurinhas que foi utilizada no album em forma percentual

                    // test package vs album
                    foreach (int stickers in stickerPackage) // vou abrir o pacote :)
                        if (!stickerAlbum[stickers]) // se não tiver ainda essa figurinha
                        {
                            stickerAlbum[stickers] = true; // agora eu tenho
                            numberStickersUnfound--; // menos uma figurinha a ser achada
                            numberofStickersUsed = numberofStickersUsed + 20; // 20% para cada figurinha
                        }
                    stickerPackage.Clear(); //limpo o pacote 
                    result.used[numberofPackages] = numberofStickersUsed; // armazeno o resultado desse pacote
                }
                result.numberofPackages = numberofPackages;
                resultList.Add(result);
            
            }
            // saida para pos processamento dos dados coletados. 
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"output.txt", false))
            {
                file.Write("numberofPackages;");
                for (int tmp = 1; tmp < 4000; tmp++)
                    file.Write("Package{0};", tmp);
                file.WriteLine("");

                for (int run = 0; run < 10000; run++)
                {
                    file.Write("{0};", resultList[run].numberofPackages);
                    for (int tmp = 1; tmp<4000; tmp++)
                        file.Write("{0};", resultList[run].used[tmp]);
                    file.WriteLine("");
                }
            }

            Console.ReadKey();
            
        }
    }
}
