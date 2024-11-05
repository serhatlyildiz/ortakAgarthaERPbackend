using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static string AccessTokenCreated = "Token oluşturuldu";
        public static string UserUpdated = "Kullanıcı güncellendi";
        public static string UserDeleted = "Kullanıcı silindi";

        //------------------------------------------- Operation Claims ------------------------------------------
        public static string OperationClaimNotFound = "Rol bulunamadı";
        public static string OperationClaimAdded = "Rol eklendi";
        public static string OperationClaimDeleted = "Rol silindi";
        public static string OperationClaimUpdated = "Rol güncellendi";

        //------------------------------------------- User Operation Claims ------------------------------------------
        public static string UserOperationClaimUpdated = "Rol güncellendi";
        public static string UserOperationClaimAdded = "Rol tanımlandı";
        public static string UserOperationClaimDeleted = "Rol kaldırıldı";
    }
}
