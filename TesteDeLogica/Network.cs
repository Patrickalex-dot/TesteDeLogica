using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;


namespace TesteDeLogica
{
    internal class Network
    {
        //Rede de bolinhas 
        private readonly List<Bolinhas> RedeDeBolinhas = new List<Bolinhas>();
        
        //Construtor que adiciona bolinhas na rede
        public Network(int tamanho)
        {
            for(int i = 1; i <= tamanho; i++)
            {
                if(tamanho < 0 || typeof(int) != tamanho.GetType())
                {
                    throw new Exception("Tipo de valores invalido");
                }

                Bolinhas bolinhas = new Bolinhas();

                bolinhas.Node = i;
                bolinhas.ListaNos = new List<Bolinhas>();
                RedeDeBolinhas.Add(bolinhas);
            }
        } 

        //Metodo que conecta Indice de uma bolinha da rede em outra bolinha
        public void Connect (int origem, int destino)
        {
            //Estoura exceprion caso origem e destino retornem um valor falso, indicando que o valor não atende aos requisitos
            if (!AnalisaTipoBolinha(origem) || !AnalisaTipoBolinha(destino))
            {
                throw new Exception("Elementos invalidos para ligação");
            }

            //Procura a raiz da origem e do destino na rede de bolinhas
            Bolinhas rootA = RedeDeBolinhas.Where(x => x.Node == origem).First();
            Bolinhas rootB = RedeDeBolinhas.Where(x => x.Node == destino).First();

            //valida se as raizes são tiferentes assim fazendo a conexão nas suas respectivas listas do objeto Bolinhas
            if (rootA != rootB)
            {
                rootA.ListaNos.Add(rootB);
                rootB.ListaNos.Add(rootA);
            }

        }
        

        //Metodo que analisa se o tipo dos elementos atendem aos requisitos
        public bool AnalisaTipoBolinha(int elemento)
        {
            
            return elemento >= 0 && elemento <= RedeDeBolinhas.Count;
        }
        
        //Metodo query que valida se duas bolinhas estão conectadas tando diretamente quanto indiretamente
        public bool Query (int bolinha1, int bolinha2)
        {
            //Estoura exceprion caso bolinha1 ou bolinha 2 não sejam inteiros
            if (bolinha1 < 0 || bolinha2 < 0)
            {
                throw new Exception("valores invalidos");
            }

            //Procura ponto1 e ponto 2 na rede de bolinhas
             Bolinhas ponto1 =  RedeDeBolinhas.Where(x => x.Node == bolinha1).FirstOrDefault();
             Bolinhas ponto2 = RedeDeBolinhas.Where(y => y.Node == bolinha2).FirstOrDefault();

            //Valida se estão conectados, se ponto1 se encontra na lista do objeto Bolinhas e vice versa
                bool A =  ponto1.ListaNos.Any(x => x.Node == ponto2.Node);
                bool B = ponto2.ListaNos.Any(y => y.Node == ponto1.Node);

            //caso seja uma conexão direta retorna true
            if(A || B)
            {
                return true;
            }

            //Guarda conexões da lista do Objeto Bolinhas que veio por parametro do ponto1
            var itensAnalisar = ponto1?.ListaNos;
            List<Bolinhas> pontosPercorridos = new List<Bolinhas>();

            //separa os itens analisados e verifica as conexões adjacentes da lista de conexões 
            while ((pontosPercorridos == null || !pontosPercorridos.Any()) &&
                    (itensAnalisar != null || itensAnalisar.Any()))
            {
                // Marcar os elementos como analisados
                foreach (var item in itensAnalisar)
                {
                    item.JaAnalisado = true;
                }

                
                itensAnalisar = BuscaConexaoSecundaria(itensAnalisar);

                // Verificar se a lista está vazia ou contém apenas elementos já analisados
                if (!itensAnalisar.Any() || itensAnalisar.All(x => x.JaAnalisado))
                    break; // Interromper o loop

                pontosPercorridos.AddRange(VerSePossuiVinculo(itensAnalisar, ponto2));
            }

            if (pontosPercorridos.Any())
                return true;
            return false;
        }

        //
        public List<Bolinhas> BuscaConexaoSecundaria(List<Bolinhas> pontosPercorridos)
        {
            var conexoesSecundarias = new List<Bolinhas>();
            var novosPontos = new List<Bolinhas>(); // Nova lista para armazenar os novos pontos a serem adicionados

            pontosPercorridos.ForEach(e =>
            {
                e.JaAnalisado = true;
                novosPontos.AddRange(BuscarNaoAnalisados(e));
            });

            pontosPercorridos.AddRange(novosPontos); // Adicionar os novos pontos à lista pontosPercorridos

            return pontosPercorridos.Distinct(new BolinhasComparer()).ToList();
        }

        // Comparador personalizado para comparar objetos Bolinhas pelo campo Node
        private class BolinhasComparer : IEqualityComparer<Bolinhas>
        {
            public bool Equals(Bolinhas x, Bolinhas y)
            {
                return x.Node == y.Node;
            }

            public int GetHashCode(Bolinhas obj)
            {
                return obj.Node.GetHashCode();
            }
        }

        
        public List<Bolinhas> BuscarNaoAnalisados(Bolinhas bolinhas)
        {
            return bolinhas?.ListaNos.Where(x => x.JaAnalisado == false)?.ToList()!;
        }

        public List<Bolinhas> VerSePossuiVinculo(List<Bolinhas> bolinhas, Bolinhas pAnalisar)
        {
            return bolinhas.Where(x => x.Node == pAnalisar.Node).ToList();
        }
    }
}
