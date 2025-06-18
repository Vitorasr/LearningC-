using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Net.NetworkInformation;
using Microsoft.Management.Infrastructure;

namespace Files.SystemMonitor;

public class SystemMonitor
{
    private PerformanceCounter cpuCounter;
    private List<string> nicNames;
    private List<PerformanceCounter> sentCounters;
    private List<PerformanceCounter> receivedCounters;

    public SystemMonitor()
    {
        cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        cpuCounter.NextValue();

        var nics = NetworkInterface.GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up && !nic.Name.Contains("Loopback"))
            .ToList();

        nicNames = nics.Select(nic => nic.Name).ToList();
        ListNetworkInstances();

        var category = new PerformanceCounterCategory("Network Interface");
        var validInstances = category.GetInstanceNames();

        sentCounters = new List<PerformanceCounter>();
        receivedCounters = new List<PerformanceCounter>();

        var nicMapping = new Dictionary<string, string>
        {
            { "Wi-Fi", "Intel(R) Wi-Fi 6E AX211 160MHz" },
            { "Ethernet", "Realtek USB GbE Family Controller" }
        };
        foreach (var nicName in nicNames)
        {
            var performanceCounterName = nicMapping.ContainsKey(nicName) ? nicMapping[nicName] : nicName;
            if (validInstances.Any(v => v.Equals(performanceCounterName, StringComparison.OrdinalIgnoreCase)))
            {
                sentCounters.Add(new PerformanceCounter("Network Interface", "Bytes Sent/sec", performanceCounterName));
                receivedCounters.Add(new PerformanceCounter("Network Interface", "Bytes Received/sec", performanceCounterName));
            }
            else
            {
                Console.WriteLine($"Instância '{performanceCounterName}' não encontrada no PerformanceCounter. Pulando...");
            }
        }

        if (sentCounters.Any() && receivedCounters.Any())
        {
            Thread.Sleep(1000);
            foreach (var counter in sentCounters.Concat(receivedCounters))
            {
                try
                {
                    counter.NextValue();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao inicializar contador de rede: {ex.Message}");
                }
            }
        }
    }

    private void ListNetworkInstances()
    {
        var category = new PerformanceCounterCategory("Network Interface");
        var instances = category.GetInstanceNames();
        foreach (var instance in instances)
        {
            Console.WriteLine($"Instância disponível: {instance}");
        }
    }

    public float GetCurrentCpuUsage()
    {
        Thread.Sleep(1000);
        return cpuCounter.NextValue();
    }

    public (ulong totalMB, ulong freeMB) GetMemoryUsage()
    {
        try
        {
            using (var session = CimSession.Create(null))
            {
                var query = "SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem";
                var results = session.QueryInstances("root\\cimv2", "WQL", query);
                foreach (var instance in results)
                {
                    ulong totalKB = Convert.ToUInt64(instance.CimInstanceProperties["TotalVisibleMemorySize"].Value);
                    ulong freeKB = Convert.ToUInt64(instance.CimInstanceProperties["FreePhysicalMemory"].Value);
                    return (totalKB / 1024, freeKB / 1024);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao consultar memória: {ex.Message}");
        }
        return (0, 0);
    }

    public void PrintCpuTemperature()
    {
        try
        {
            using (var session = CimSession.Create(null))
            {
                var query = "SELECT * FROM Sensor WHERE SensorType = 'Temperature' AND Parent LIKE '%CPU%'";
                var results = session.QueryInstances("root\\LibreHardwareMonitor", "WQL", query);
                if (!results.Any())
                {
                    Console.WriteLine("Nenhuma temperatura encontrada. Verifique se o LibreHardwareMonitor está em execução como administrador.");
                    return;
                }

                double coreAverage = 0;
                double coreMax = double.MinValue;
                double? packageTemp = null;
                int coreCount = 0;

                foreach (var instance in results)
                {
                    var name = instance.CimInstanceProperties["Name"].Value?.ToString() ?? "Desconhecido";
                    var value = Convert.ToDouble(instance.CimInstanceProperties["Value"].Value);
                    if (name.Contains("Core Max"))
                    {
                        coreMax = value;
                    }
                    else if (name.Contains("CPU Package"))
                    {
                        packageTemp = value;
                    }
                    else if (name.StartsWith("CPU Core #") && !name.Contains("Distance to TjMax"))
                    {
                        coreCount++;
                        coreAverage += value;
                        coreMax = Math.Max(coreMax, value);
                    }
                }

                if (packageTemp.HasValue)
                {
                    Console.Write($"CPU Package: {packageTemp:F2}°C");
                }
                if (coreAverage > 0 && coreCount > 0)
                {
                    coreAverage /= coreCount;

                    Console.Write($" Core Average: {coreAverage:F2}°C");
                }
                if (coreMax > double.MinValue)
                {
                    Console.Write($" Core Max: {coreMax:F2}°C");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao consultar temperatura da CPU: {ex.Message}");
        }
    }
    public void PrintGpuUsage()
    {
        try
        {
            using (var session = CimSession.Create(null))
            {
                var query = "SELECT * FROM Sensor WHERE SensorType = 'Load' AND Parent LIKE '%GPU%'";
                var results = session.QueryInstances("root\\LibreHardwareMonitor", "WQL", query);
                if (!results.Any())
                {
                    Console.WriteLine("Nenhum dado de uso encontrado. Verifique se o LibreHardwareMonitor está em execução como administrador.");
                    return;
                }
                foreach (var instance in results)
                {
                    var name = instance.CimInstanceProperties["Name"].Value?.ToString() ?? "Desconhecido";
                    var value = Convert.ToDouble(instance.CimInstanceProperties["Value"].Value);
                    if (name.Contains("GPU Core"))
                    {
                        Console.WriteLine($"GPU Core: {value:F2}%");
                        break; // Exibe apenas o principal
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao consultar uso da GPU: {ex.Message}");
        }
    }

    public void PrintGpuTemperature()
    {
        try
        {
            using (var session = CimSession.Create(null))
            {
                var query = "SELECT * FROM Sensor WHERE SensorType = 'Temperature' AND Parent LIKE '%GPU%'";
                var results = session.QueryInstances("root\\LibreHardwareMonitor", "WQL", query);
                if (!results.Any())
                {
                    Console.WriteLine("Nenhuma temperatura encontrada. Verifique se o LibreHardwareMonitor está em execução como administrador.");
                    return;
                }
                foreach (var instance in results)
                {
                    var name = instance.CimInstanceProperties["Name"].Value?.ToString() ?? "Desconhecido";
                    var value = Convert.ToDouble(instance.CimInstanceProperties["Value"].Value);
                    if (name.Contains("GPU Core"))
                    {
                        Console.WriteLine($"GPU Core: {value:F2}°C");
                        break; // Exibe apenas o principal
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao consultar temperatura da GPU: {ex.Message}");
        }
    }

    public void PrintDiskUsage()
    {
        foreach (var drive in DriveInfo.GetDrives())
        {
            if (drive.IsReady)
            {
                long totalGB = drive.TotalSize / (1024 * 1024 * 1024);
                long freeGB = drive.AvailableFreeSpace / (1024 * 1024 * 1024);
                float usagePercent = (float)(totalGB - freeGB) / totalGB * 100;
                Console.WriteLine($"{drive.Name}: {freeGB} GB livres de {totalGB} GB ({usagePercent:F2}% usado)");
            }
        }
    }

    public void PrintNetworkUsage()
    {
        if (!sentCounters.Any() || !receivedCounters.Any())
        {
            Console.WriteLine("Nenhuma interface de rede encontrada ou configurada.");
            return;
        }

        for (int i = 0; i < sentCounters.Count; i++)
        {
            try
            {
                float sent = sentCounters[i].NextValue();
                float received = receivedCounters[i].NextValue();
                Console.WriteLine($"{nicNames[i]}: Enviado {sent:F2} bytes/seg, Recebido {received:F2} bytes/seg");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler dados da rede {nicNames[i]}: {ex.Message}");
            }
        }
    }

    public void PrintTopProcesses(int topN = 10)
    {
        try
        {
            var processes = Process.GetProcesses().OrderByDescending(p => p.WorkingSet64).Take(topN);
            foreach (var p in processes)
            {
                Console.WriteLine($"{p.ProcessName}: {p.WorkingSet64 / (1024 * 1024):F2} MB");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao listar processos: {ex.Message}");
        }
    }
}