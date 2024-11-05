using System;
using System.Diagnostics;
using Castle.DynamicProxy;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class PerformanceAspect : Attribute, IInterceptor
{
    private readonly int _interval; // Zaman eşiği (ms cinsinden)
    private Stopwatch _stopwatch;

    public PerformanceAspect(int interval)
    {
        _interval = interval;
        _stopwatch = new Stopwatch();
    }

    public void Intercept(IInvocation invocation)
    {
        _stopwatch.Start(); // Süre ölçümü başlatılıyor

        // Asıl metodu çalıştır
        invocation.Proceed();

        _stopwatch.Stop(); // Süre ölçümü durduruluyor

        // Süre kontrolü: Belirlenen eşiği aştıysa logla veya uyar
        if (_stopwatch.ElapsedMilliseconds > _interval)
        {
            Console.WriteLine($"Performance Warning: {invocation.Method.Name} took {_stopwatch.ElapsedMilliseconds} ms, exceeding the limit of {_interval} ms.");
            // İsteğe bağlı olarak burada loglama yapılabilir
        }

        _stopwatch.Reset(); // Ölçüm sayacını sıfırla
    }
}
