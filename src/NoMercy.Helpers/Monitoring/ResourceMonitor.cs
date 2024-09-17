using System.Runtime.InteropServices;
using LibreHardwareMonitor.Hardware;
using Newtonsoft.Json;
using NoMercy.NmSystem;

namespace NoMercy.Helpers.Monitoring;

public class Resource
{
    [JsonProperty("cpu")] public Cpu Cpu { get; set; } = new();
    internal Dictionary<Identifier, Gpu> _gpu { get; set; } = [];
    [JsonProperty("memory")] public Memory Memory { get; set; } = new();
    [JsonProperty("gpu")] public List<Gpu> Gpu => _gpu.Values.ToList();
}

public class Cpu
{
    [JsonProperty("total")] public double Total { get; set; }
    [JsonProperty("max")] public double Max { get; set; }
    [JsonProperty("core")] public List<Core> Core { get; set; } = [];
}

public class Core
{
    [JsonProperty("index")] public int Index { get; set; }
    [JsonProperty("utilization")] public double Utilization { get; set; }
}

public class Gpu
{
    [JsonProperty("d3d")] public double D3D { get; set; }
    [JsonProperty("decode")] public double Decode { get; set; }
    [JsonProperty("core")] public double Core { get; set; }
    [JsonProperty("memory")] public double Memory { get; set; }
    [JsonProperty("encode")] public double Encode { get; set; }
    [JsonProperty("power")] public double Power { get; set; }
    [JsonProperty("identifier")] internal Identifier Identifier { get; set; } = new();
    [JsonProperty("index")] public int Index => int.Parse(Identifier.ToString().Split('/').LastOrDefault() ?? "0");
}

public class Memory
{
    [JsonProperty("available")] public double Available { get; set; }
    [JsonProperty("use")] public double Use { get; set; }
    [JsonProperty("total")] public double Total { get; set; }

    [JsonProperty("percentage")]
    public double Percentage =>
        Use / Total * (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? 0.1 : 100);
}

public class ResourceMonitor
{
    private static Computer? _computer;

    static ResourceMonitor()
    {
        Logger.App("Initializing Resource Monitor");
        if (_computer is not null) return;
        Logger.App("Creating new computer instance");
        _computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
            IsMotherboardEnabled = false,
            IsControllerEnabled = false,
            IsNetworkEnabled = false,
            IsStorageEnabled = false
        };
        _computer.Open();
    }

    private class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }

        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware? subHardware in hardware.SubHardware) subHardware.Accept(this);
        }

        public void VisitSensor(ISensor sensor)
        {
        }

        public void VisitParameter(IParameter parameter)
        {
        }
    }

    public static Resource? Monitor()
    {
        try
        {
            if (_computer is null) return null;

            _computer.Accept(new UpdateVisitor());

            Resource resource = new()
            {
                Cpu = new Cpu
                {
                    Core = []
                },
                _gpu = new Dictionary<Identifier, Gpu>(),
                Memory = new Memory()
            };

            foreach (IHardware? hardware in _computer.Hardware)
            {
                if (hardware.HardwareType is not HardwareType.Cpu and not HardwareType.Memory
                    and not HardwareType.GpuIntel and not HardwareType.GpuNvidia and not HardwareType.GpuAmd
                   ) continue;

                try
                {
                    foreach (ISensor? sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType is not SensorType.Load && sensor.SensorType is not SensorType.Data)
                            continue;

                        // Logger.App($"Type: {sensor.Hardware}, Identifier: {sensor.Hardware.Identifier}, Sensor: {sensor.Name}, value: {sensor.Value}");
                        switch (sensor.Hardware.HardwareType)
                        {
                            case HardwareType.Cpu:
                                switch (sensor.Name)
                                {
                                    case "CPU Total":
                                        resource.Cpu.Total = sensor.Value ?? 0.0;
                                        break;
                                    case "CPU Core Max":
                                        resource.Cpu.Max = sensor.Value ?? 0.0;
                                        break;
                                    default:
                                        resource.Cpu.Core.Add(new Core
                                        {
                                            Index = sensor.Index - 1,
                                            Utilization = sensor.Value ?? 0.0
                                        });
                                        break;
                                }

                                break;
                            case HardwareType.Memory:
                                switch (sensor.Name)
                                {
                                    case "Memory Available":
                                        resource.Memory.Available = sensor.Value ?? 0.0;
                                        break;
                                    case "Memory Used":
                                        resource.Memory.Use = sensor.Value ?? 0.0;
                                        break;
                                    case "Virtual Memory":
                                        resource.Memory.Total = sensor.Value ?? 0.0;
                                        break;
                                }

                                break;
                            case HardwareType.GpuNvidia:
                                KeyValuePair<Identifier, Gpu> gpu = resource._gpu
                                    .FirstOrDefault(g => g.Key == sensor.Hardware.Identifier);

                                switch (sensor.Name)
                                {
                                    case "GPU Video Engine":
                                        if (gpu.Value is not null)
                                            gpu.Value.Core = sensor.Value ?? 0;
                                        else
                                            resource._gpu[sensor.Hardware.Identifier] = new Gpu
                                            {
                                                Core = sensor.Value ?? 0,
                                                Identifier = sensor.Hardware.Identifier
                                            };
                                        break;
                                    case "D3D Video Decode":
                                        if (gpu.Value is not null)
                                            gpu.Value.Decode = sensor.Value ?? 0;
                                        else
                                            resource._gpu[sensor.Hardware.Identifier] = new Gpu
                                            {
                                                Decode = sensor.Value ?? 0,
                                                Identifier = sensor.Hardware.Identifier
                                            };
                                        break;
                                    case "D3D Video Encode":
                                        if (gpu.Value is not null)
                                            gpu.Value.Encode = sensor.Value ?? 0;
                                        else
                                            resource._gpu[sensor.Hardware.Identifier] = new Gpu
                                            {
                                                Encode = sensor.Value ?? 0,
                                                Identifier = sensor.Hardware.Identifier
                                            };
                                        break;
                                    case "D3D 3D":
                                        if (gpu.Value is not null)
                                            gpu.Value.D3D = sensor.Value ?? 0;
                                        else
                                            resource._gpu[sensor.Hardware.Identifier] = new Gpu
                                            {
                                                D3D = sensor.Value ?? 0,
                                                Identifier = sensor.Hardware.Identifier
                                            };
                                        break;
                                    case "GPU Memory":
                                        if (gpu.Value is not null)
                                            gpu.Value.Memory = sensor.Value ?? 0;
                                        else
                                            resource._gpu[sensor.Hardware.Identifier] = new Gpu
                                            {
                                                Memory = sensor.Value ?? 0,
                                                Identifier = sensor.Hardware.Identifier
                                            };
                                        break;
                                    case "GPU Power":
                                        if (gpu.Value is not null)
                                            gpu.Value.Power = sensor.Value ?? 0;
                                        else
                                            resource._gpu[sensor.Hardware.Identifier] = new Gpu
                                            {
                                                Power = sensor.Value ?? 0,
                                                Identifier = sensor.Hardware.Identifier
                                            };
                                        break;
                                }

                                break;
                            case HardwareType.GpuIntel:
                                break;
                            case HardwareType.GpuAmd:
                                break;
                            case HardwareType.Motherboard:
                                break;
                            case HardwareType.SuperIO:
                                break;
                            case HardwareType.Storage:
                                break;
                            case HardwareType.Network:
                                break;
                            case HardwareType.Cooler:
                                break;
                            case HardwareType.EmbeddedController:
                                break;
                            case HardwareType.Psu:
                                break;
                            case HardwareType.Battery:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    ;
                }
                catch
                {
                    throw new Exception("Error while monitoring hardware");
                }
            }

            return resource;
        }
        catch (Exception e)
        {
            //
        }

        return null;
    }

    public static void Start()
    {
        _computer?.Open();
    }

    public static void Stop()
    {
        _computer?.Close();
    }

    ~ResourceMonitor()
    {
        _computer?.Close();
    }
}