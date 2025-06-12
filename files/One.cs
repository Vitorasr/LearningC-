using System.Collections.Generic;
namespace LearningCSharp
{
    public class Pessoa
    {
        public int Idade;
        public double Altura;
        public string Nome;
        public bool Progamador;
        public void Show()
        {
            Console.WriteLine("Nome: " + Nome);
            Console.WriteLine("Altura: " + Altura);
            Console.WriteLine("Idade: " + Idade);
            Console.WriteLine("É um progamador? " + Progamador);
        }
    }
    public class One
    {
        public static Pessoa Vars()
        {
            Pessoa pessoa = new Pessoa
            {
                Idade = 18,
                Altura = 1.77,
                Nome = "Vitor",
                Progamador = true
            };

            Console.WriteLine("Nome: " + pessoa.Nome);
            Console.WriteLine("Altura: " + pessoa.Altura);
            Console.WriteLine("Idade: " + pessoa.Idade);
            Console.WriteLine("É um progamador? " + pessoa.Progamador);
            return pessoa;

        }

        public static void Condicionais(Pessoa pessoa)
        {
            if (pessoa.Idade >= 18)
            {
                Console.WriteLine("Você é maior de idade");
            }
            else
            {
                Console.WriteLine("Você é menor de idade");
            }

            if (pessoa.Progamador)
            {
                Console.WriteLine("Você é um progamador");
            }
            else
            {
                Console.WriteLine("Você não é um progamador");
            }
            if (pessoa.Altura >= 1.75)
            {
                Console.WriteLine("Você é alto");
            }
            else
            {
                Console.WriteLine("Você é baixo");
            }
        }
        public static void Arrays()
        {
            int[] numeros = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int soma = 0;
            for (int i = 0; i < numeros.Length; i++)
            {
                soma += numeros[i];
            }
            Console.WriteLine("A soma dos números é: " + soma);
        }

        public static void Object()
        {
            Pessoa pessoa = new Pessoa();
            pessoa.Nome = "Vitor";
            pessoa.Idade = 18;
            pessoa.Altura = 1.77;
            pessoa.Progamador = true;
            pessoa.Show();
        }
        public static List<string> List()
        {
            List<string> nomes = new List<string>();
            nomes.Add("Vitor");
            nomes.Add("João");
            nomes.Add("Maria");
            nomes.Add("José");
            nomes.Add("Ana");


            Console.WriteLine("Lista de nomes:");
            foreach (string nome in nomes)
            {
                Console.WriteLine(nome);
            }
            return nomes;
        }
        public static void Dictionary()
        {
            Dictionary<string, int> idades = new Dictionary<string, int>();
            idades.Add("Vitor", 18);
            idades.Add("João", 20);
            idades.Add("Maria", 19);
            idades.Add("José", 21);
            List<string> nomes = One.List();

            Console.WriteLine("Dicionário de idades:");
            foreach (string nome in nomes)
            {   if (idades.ContainsKey(nome))
                {
                    int idade = idades[nome];
                    Console.WriteLine(nome + ": " + idade);
                }
                else
                {
                    Console.WriteLine(nome + ": Não encontrado");
                }
            }
            
        }
    }
}