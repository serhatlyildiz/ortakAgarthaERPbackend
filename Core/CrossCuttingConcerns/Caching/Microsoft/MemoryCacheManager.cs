using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Text.RegularExpressions;

public class MemoryCacheManager : ICacheManager
{
    private IMemoryCache _memoryCache;
    private const string CacheKeyList = "CacheKeyList";

    public MemoryCacheManager()
    {
        _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
    }

//    public class Sepet
//    {
//        public int SepetID { get; set; }
//        public int KullaniciID { get; set; }
//        public List<Malzeme> sepetiniçindemalzemer  { get; set; }
//}

    public void Add(string key, object value, int duration)
    {
        _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));

        // Anahtar listesini güncelle
        var keys = _memoryCache.Get<List<string>>(CacheKeyList) ?? new List<string>();


        //var s = new Sepet
        //{
        //    SepetID = 1,
        //    KullaniciID = 1,
        //    sepetiniçindemalzemer = ["kalem", "kitap"],
        //};

        
        //_memoryCache.Set($"sepet{s.KullaniciID}", s, TimeSpan.FromMinutes(10));

        //var test = _memoryCache.Get<Sepet>($"lstSepet{s.KullaniciID}");
        if (!keys.Contains(key))
        {
            keys.Add(key);
            _memoryCache.Set(CacheKeyList, keys);
        }
    }

    public T Get<T>(string key)
    {
        return _memoryCache.Get<T>(key);
    }

    public object Get(string key)
    {
        return _memoryCache.Get(key);
    }

    public bool IsAdd(string key)
    {
        return _memoryCache.TryGetValue(key, out _);
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);

        // Anahtar listesinden çıkar
        var keys = _memoryCache.Get<List<string>>(CacheKeyList) ?? new List<string>();
        keys.Remove(key);
        _memoryCache.Set(CacheKeyList, keys);
    }

    public void RemoveByPattern(string pattern)
    {
        var keys = _memoryCache.Get<List<string>>(CacheKeyList) ?? new List<string>();
        var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        var keysToRemove = keys.Where(k => regex.IsMatch(k)).ToList();
        foreach (var key in keysToRemove)
        {
            _memoryCache.Remove(key);
            keys.Remove(key);
        }

        // Anahtar listesini güncelle
        _memoryCache.Set(CacheKeyList, keys);
    }
}
