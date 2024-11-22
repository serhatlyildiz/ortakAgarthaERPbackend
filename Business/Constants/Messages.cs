namespace Business.Constants
{
    public static class Messages
    {
        //------------------------------------------- PRODUCT ------------------------------------------
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün ismi geçersiz";
        public static string MaintenanceTime = "Sistem bakımda";
        public static string ProductsListed = "Ürünler listelendi";
        public static string ProductCountOfCategoryError = "Bir kategoride en fazla 10 ürün olabilir";
        public static string ProductNameAlreadyExists = "Bu isimde zaten başka bir ürün var";
        public static string ProductNotFound = "Ürün bulunamadı";
        public static string ProductDeleted = " ürünü silindi";
        public static string ProductUpdated = "Ürün güncellendi";

        //------------------------------------------- CATEGORY ------------------------------------------
        public static string CategoryLimitExceded = "Kategori limiti aşıldığı için yeni ürün eklenemiyor";
        public static string CategoryNameAlreadyExists = "Bu isimde zaten başka bir kategori var";
        public static string CategoryAdded = "Kategori eklendi";
        public static string CategoryNotFound = "Ürün bulunamadı";
        public static string CategoryUpdated = "Kategori güncellendi";
        public static string CategoryDeleted = " ürünü silindi";

        //------------------------------------------- AUTH (USERS) ------------------------------------------
        public static string AuthorizationDenied = "Yetkiniz yok";
        public static string UserRegistered = "Kayıt oldu";
        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordError = "Parola hatası";
        public static string SuccessfulLogin = "Başarılı giriş";
        public static string UserAlreadyExists = "Kullanıcı mevcut";
        public static string AccessTokenCreated = "Giriş başarılı";
        public static string UserUpdated = "Kullanıcı güncellendi";
        public static string UserDeleted = "Kullanıcı silindi";

        //------------------------------------------- OPERATION CLAIMS ------------------------------------------
        public static string OperationClaimNotFound = "Rol bulunamadı";
        public static string OperationClaimAdded = "Rol eklendi";
        public static string OperationClaimDeleted = "Rol silindi";
        public static string OperationClaimUpdated = "Rol güncellendi";
        public static string OperationClaimListed = "Roller listelendi";

        //------------------------------------------- USER OPERATION CLAIMS ------------------------------------------
        public static string UserOperationClaimUpdated = "Rol güncellendi";
        public static string UserOperationClaimAdded = "Rol tanımlandı";
        public static string UserOperationClaimDeleted = "Rol kaldırıldı";
        public static string UserOperationClaimListed = "Roller listelendi";

        //----------------------------------------------ORDER------------------------------------------------------
        public static string OrderAdded = "Sipariş oluşturuldu";
        public static string OrdersListed = "Siparişler Listelendi";
        public static string OrderUpdated = "Sipariş güncellendi";
        public static string OrderDeleted = " numaralı siparişiniz iptal edilmiştir";

        //--------------------------------------------PRODUCTSTOCKS-----------------------------------------------
        public static string ProductStocksAdded = "Stoğa Eklendi";
        public static string ProductStocksListed = "Stoklar Listelendi";
        public static string ProductStocksNotFound = "Stok bulunamadı";
        public static string ProductStocksUpdated = "Stok güncellendi";
        public static string ProductStocksDeleted = "Stok bilgisi silindi";

        //---------------------------------------------COLORS--------------------------------------------------------
        public static string ColorAdded = "Renk eklendi";
        public static string ColorDeleted = "Renk silindi";
        public static string ColorUpdated = "Renk güncellendi";
        public static string ColorNotFound = "Renk bulunamadı";
        public static string ColorNameAlreadyExists = "Renk zaten var";

        //-------------------------------------------FORGOT-PASS-----------------------------------------------------
        public static string PasswordResetRequested = "Şifre sıfırlama isteği alındı. E-postanızı kontrol edin.";
        public static string InvalidOrExpiredToken = "Geçersiz veya süresi dolmuş bir token.";
        public static string PasswordResetSuccessful = "Şifre başarıyla sıfırlandı.";

    }
}