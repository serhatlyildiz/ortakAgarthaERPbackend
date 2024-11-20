namespace Core.Utilities.Helpers
{
    public class GoogleOAuthHelper
    {
        //private static string[] Scopes = { GmailService.Scope.GmailSend };
        //private static string ApplicationName = "Web Application";

        //public static GmailService GetGmailService()
        //{
        //    UserCredential credential;

        //    // credential.json dosyasını kullanarak kimlik doğrulaması yapılır
        //    using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        //    {
        //        // token dosyasına kaydedilmesi için belirtilen dizin
        //        string credPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".credentials/gmail-dotnet-quickstart.json");

        //        // OAuth2 kimlik doğrulaması yapılır
        //        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //            GoogleClientSecrets.Load(stream).Secrets,
        //            Scopes,
        //            "user",
        //            CancellationToken.None,
        //            new FileDataStore(credPath, true)).Result;
        //    }

        //    // Gmail API'ye bağlanılır
        //    var service = new GmailService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = ApplicationName,
        //    });

        //    return service;
        //}
    }
}