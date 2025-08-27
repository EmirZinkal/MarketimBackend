namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün başarıyla eklendi";
        public static string ProductDeleted = "Ürün başarıyla silindi";
        public static string ProductUpdated = "Ürün başarıyla güncellendi";
        public static string ProductNameNotEmpty = "Ürün adı boş olamaz";
        public static string ProductNameMinLength = "Ürün adı en az 2 karakter olmalı";
        public static string ProductPriceGreaterThanZero = "Fiyat sıfırdan büyük olmalı";
        public static string ProductMarketIdRequired = "Geçerli bir market seçmelisiniz";
        public static string ProductNameAlreadyExistsInMarket = "Bu markette bu ürün zaten var.";
        public static string MarketNotFound = "Geçerli bir market seçiniz.";
        public static string ProductNotFound = "Ürün bulunamadı.";

        public static string UserAdded = "Kullanıcı başarıyla eklendi";
        public static string UserDeleted = "Kullanıcı başarıyla silindi";
        public static string UserUpdated = "Kullanıcı başarıyla güncellendi";

        public static string ShoppingListAdded = "Alışveriş kaydı başarıyla eklendi";
        public static string ShoppingListDeleted = "Alışveriş kaydı başarıyla silindi";
        public static string ShoppingListUpdated = "Alışveriş kaydı başarıyla güncellendi";
        public static string ShoppingListTitleNotEmpty = "Liste başlığı boş olamaz";
        public static string ShoppingListUserIdRequired = "Geçerli bir kullanıcı seçmelisiniz";
        public static string ShoppingListNotFound = "Alışveriş listesi bulunamadı.";


        public static string MarketAdded = "Market kaydı başarıyla eklendi";
        public static string MarketDeleted = "Market kaydı başarıyla silindi";
        public static string MarketUpdated = "Market kaydı başarıyla güncellendi";
        public static string MarketNameNotEmpty = "Market adı boş olamaz";
        public static string MarketNameMinLength = "Market adı en az 2 karakter olmalı";

        public static string NotificationAdded = "Bildirim başarıyla eklendi";
        public static string NotificationDeleted = "Bildirim başarıyla silindi";
        public static string NotificationUpdated = "Bildirim başarıyla güncellendi";

        public static string MessageAdded = "Mesaj başarıyla eklendi";
        public static string MessageDeleted = "Mesaj başarıyla silindi";
        public static string MessageUpdated = "Mesaj başarıyla güncellendi";
        public static string MessageContentNotEmpty = "Mesaj içeriği boş olamaz";
        public static string MessageContentMaxLength = "Mesaj 500 karakterden uzun olamaz";
        public static string MessageSenderIdRequired = "Geçerli bir gönderen seçmelisiniz";
        public static string MessageReceiverIdRequired = "Geçerli bir alıcı seçmelisiniz";
        public static string MessageNotFound = "Mesaj bulunamadı.";
        public static string MessageSenderReceiverCannotBeSame = "Gönderici ve alıcı aynı olamaz.";

        public static string FriendAdded = "Arkadaş başarıyla eklendi";
        public static string FriendDeleted = "Arkadaş başarıyla silindi";
        public static string FriendUpdated = "Arkadaş başarıyla güncellendi";
        public static string FriendUserIdRequired = "Geçerli bir kullanıcı seçmelisiniz";
        public static string FriendFriendIdRequired = "Geçerli bir arkadaş seçmelisiniz";
        public static string FriendNotFound = "Arkadaş kaydı bulunamadı.";
        public static string FriendCannotAddSelf = "Kendinizi arkadaş olarak ekleyemezsiniz.";
        public static string FriendAlreadyExists = "Bu kişi zaten arkadaş listenizde.";

        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordError="Şifre hatalı";
        public static string SuccessfulLogin="Sisteme giriş başarılı";
        public static string UserAlreadyExists = "Bu kullanıcı zaten mevcut";
        public static string UserRegistered = "Kullanıcı başarıyla kaydedildi";
        public static string AccessTokenCreated= "Access token başarıyla oluşturuldu";

        public static string PasswordResetEmailSent = "Şifre sıfırlama maili gönderildi!";

        public static string PasswordChanged = "Şifre Değiştirildi.";

        public static string InvalidOrExpiredToken = "Geçersiz veya süresi geçmiş token!";

        public static string UserAlreadyHasThisRole = "Kullanıcı zaten bu role sahip";
        public static string UserRoleUpdated = "Kullanıcı rolü güncellendi";
        public static string RoleAdded = "Kullanıcı rolü güncellendi";

        public static string MessageSendTooFrequently = "Çok sık messaj gönderilemez";
    }
}
