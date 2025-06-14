
namespace CalculatorFile
{
    public class CalculatorClass
    {
        public static void init()
        {
            while (true)
            {
                Console.WriteLine("=== Calculadora Científica: Pressione a tecla correspondente :)");
                Console.WriteLine("1 - Soma");
                Console.WriteLine("2 - Subtração");
                Console.WriteLine("3 - Multiplicação");
                Console.WriteLine("4 - Divisão");
                Console.WriteLine("5 - Potência");
                Console.WriteLine("6 - Raiz Quadrada");
                Console.WriteLine("7 - Seno");
                Console.WriteLine("8 - Cosseno");
                Console.WriteLine("9 - Tangente");
                Console.WriteLine("10 - Logaritmo(Base 10)");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha uma opção: ");

                int type = int.Parse(Console.ReadLine());
                if (type == 0) break;

                double num1 = 0, num2 = 0, resultado = 0;
                if (type >= 1 && type <= 5)
                {
                    Console.WriteLine("Digite o primeiro número: ");
                    num1 = double.Parse(Console.ReadLine());

                    Console.WriteLine("Digite o segundo número: ");
                    num2 = double.Parse(Console.ReadLine());
                    double result = CalculatorClass.calculateFunction(type, num1, num2);
                    Console.WriteLine("Seu Resultado é :" + result);
                    Environment.Exit(0); 
                }
                else if (type >= 6 && type <= 10)
                {
                    Console.WriteLine("Digite o número: ");
                    num1 = double.Parse(Console.ReadLine());
                    num2 = 0;
                    double result = CalculatorClass.calculateFunction(type, num1, num2);
                    Console.WriteLine("Seu Resultado é :" + result);
                    Environment.Exit(0);
                }
            }
        }
        private static double calculateFunction(int type, double num1, double num2)
        {
            double result;

            switch (type)
            {
                case 1:
                    result = CalculatorClass.Soma(num1, num2);
                    return result;
                case 2:
                    result = CalculatorClass.Sub(num1, num2);
                    return result;
                case 3:
                    result = CalculatorClass.Multi(num1, num2);
                    return result;
                case 4:
                    result = CalculatorClass.Div(num1, num2);
                    return result;
                case 5:
                    result = CalculatorClass.potencia(num1, num2);
                    return result;
                case 6:
                    result = CalculatorClass.raizQuadrada(num1);
                    return result;
                case 7:
                    result = CalculatorClass.seno(num1);
                    return result;
                case 8:
                    result = CalculatorClass.cosseno(num1);
                    return result;
                case 9:
                    result = CalculatorClass.tangente(num1);
                    return result;
                case 10:
                    result = CalculatorClass.Logaritmo(num1);
                    return result;
                default:
                    throw new ArgumentException("Tipo de operação inválido.");
            }
        }
        private static double Soma(double num1, double num2)
        {
            double result = num1 + num2;
            return result;
        }
        private static double Sub(double num1, double num2)
        {
            double result = num1 - num2;
            return result;
        }
        private static double Multi(double num1, double num2)
        {
            double result = num1 * num2;
            return result;
        }
        private static double Div(double num1, double num2)
        {
            double result = num1 / num2;
            return result;
        }
        private static double potencia(double num1, double num2)
        {
            double result = Math.Pow(num1, num2);
            return result;
        }
        private static double raizQuadrada(double num1)
        {
            double result = Math.Sqrt(num1);
            return result;
        }
        private static double seno(double num1)
        {
            double result = Math.Sin(num1);
            return result;
        }
        private static double cosseno(double num1)
        {
            double result = Math.Cos(num1);
            return result;
        }
        private static double tangente(double num1)
        {
            double result = Math.Tan(num1);
            return result;
        }
        private static double Logaritmo(double num1)
        {
            double result = Math.Log10(num1);
            return result;
        }

    }
}