using Core.Aspects.Autofac.Caching;
using Core.Entities;
using Core.Utilities.Results;
using Newtonsoft.Json;
using RestSharp;

namespace Core.Utilities.Helpers
{
    public class SalahTime
    {
        [CacheAspect]
        public IDataResult<SalahDto> GetSalahTimes()
        {
            try
            {
                var client = new RestClient("https://api.collectapi.com/pray/all?data.city=ankara");
                var request = new RestRequest
                {
                    Method = Method.Get // HTTP metodunu burada belirtirsiniz
                };
                request.AddHeader("authorization", "apikey 31ziqQZn43zFLtWwHvvvEu:0rjAs97kCQvhXRLI5n0gnd");
                request.AddHeader("content-type", "application/json");

                // API isteğini senkron şekilde gönderiyoruz
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    // Gelen cevabı deserialize ediyoruz
                    var salahTimeResponse = JsonConvert.DeserializeObject<SalahDto>(response.Content);

                    // Başarılı sonuç dönüyoruz
                    return new SuccessDataResult<SalahDto>(salahTimeResponse, "Veriler başarıyla alındı.");
                }

                // Hata durumunda error dönüyoruz
                return new ErrorDataResult<SalahDto>(null, "API'den veri alınamadı.");
            }
            catch (Exception ex)
            {
                // Hata durumunda error dönüyoruz
                return new ErrorDataResult<SalahDto>(null, $"Hata oluştu: {ex.Message}");
            }
        }

    }

    public class SalahDto : IDto
    {
        public bool Success { get; set; }
        public List<PrayerTime> Result { get; set; }
    }
    public class PrayerTime : IDto
    {
        public string Saat { get; set; }  // Vakit saati
        public string Vakit { get; set; } // Vakit adı (İmsak, Güneş, vb.)
    }
}