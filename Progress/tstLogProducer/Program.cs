using LogService;
using Serilog;

Log.Logger = LogProducer.AddSerilog();

while (true) {
    Thread.Sleep(1000);
    Log.Warning("Ha ha");
}