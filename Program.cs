using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    class Contador
    {
        public int Id { get; private set; }
        public int Valor { get; private set; }
        public int Intervalo { get; private set; }
        public bool Activo { get; private set; }
        private Thread hilo;
        private bool detener;

        public Contador(int id, int intervalo)
        {
            Id = id;
            Valor = 0;
            Intervalo = intervalo;
            Activo = false;
        }
        public void Iniciar()
        {
            if (Activo)
            {
                Console.WriteLine($"El contador {Id} esta en ejecucion.");
                return;
            }

            detener = false;
            hilo = new Thread(Ejecutar);
            hilo.Start();
            Activo = true;
            Console.WriteLine($"✅ Contador {Id} iniciado intervalo de {Intervalo} ms.");
        }
        private void Ejecutar()
        {
            while (!detener)
            {
                Valor++;
                Console.WriteLine($"[Contador {Id}] Valor actual: {Valor}");
                Thread.Sleep(Intervalo);
            }
            Activo = false;
            Console.WriteLine($"Contador {Id} detenido.");
        }
        public void Detener()
        {
            if (!Activo)
            {
                Console.WriteLine($"El contador {Id} no esta en ejecucion.");
                return;
            }
            detener = true;
        }
    }
    static Dictionary<int, Contador> contadores = new Dictionary<int, Contador>();
    static int proximoId = 1;
    static bool salir = false;
    static void Main()
    {
        while (!salir)
        {
            Console.WriteLine("\nMENU PRINCIPAL");
            Console.WriteLine("1. Iniciar un contador");
            Console.WriteLine("2. Detener un contador");
            Console.WriteLine("3. Mostrar estado de los contadores");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opcion: ");
            string opcion = Console.ReadLine();
            switch (opcion)
            {
                case "1":
                    IniciarContador();
                    break;
                case "2":
                    DetenerContador();
                    break;
                case "3":
                    MostrarEstado();
                    break;
                case "4":
                    SalirPrograma();
                    break;
                default:
                    Console.WriteLine("Opcion desconocida, intente de nuevo.");
                    break;
            }
        }
    }
    static void IniciarContador()
    {
        Console.Write("Ingrese el intervalo en milisegundos: ");
        if (int.TryParse(Console.ReadLine(), out int intervalo))
        {
            var contador = new Contador(proximoId++, intervalo);
            contadores.Add(contador.Id, contador);
            contador.Iniciar();
        }
        else
        {
            Console.WriteLine("Intervalo no valido.");
        }
    }
    static void DetenerContador()
    {
        Console.Write("Ingrese el ID del contador que deseas detener: ");
        if (int.TryParse(Console.ReadLine(), out int id) && contadores.ContainsKey(id))
        {
            contadores[id].Detener();
        }
        else
        {
            Console.WriteLine("ID no valido o contador no encontrado.");
        }
    }
    static void MostrarEstado()
    {
        if (contadores.Count == 0)
        {
            Console.WriteLine("No hay contadores creados.");
            return;
        }

        Console.WriteLine("\nESTADO DE LOS CONTADORES");
        foreach (var kvp in contadores)
        {
            var c = kvp.Value;
            Console.WriteLine($"ID: {c.Id} | Valor: {c.Valor} | Activo: {c.Activo} | Intervalo: {c.Intervalo} ms");
        }
    }
    static void SalirPrograma()
    {
        Console.WriteLine("Deteniendo los contadores...");
        foreach (var contador in contadores.Values)
        {
            contador.Detener();
        }

        Thread.Sleep(500);
        salir = true;
        Console.WriteLine("Programa finalizado correctamente.");
    }
}
