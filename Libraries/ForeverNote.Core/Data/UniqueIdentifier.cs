using MongoDB.Bson;

namespace ForeverNote.Core.Data
{
    public static class UniqueIdentifier
    {
        public static string New => ObjectId.GenerateNewId().ToString();
    }
}
