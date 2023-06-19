using MongoDB.Bson;

namespace CaptainCommerce.DB
{
    public static class UniqueIdentifier
    {
        public static string New => ObjectId.GenerateNewId().ToString();
    }
}
