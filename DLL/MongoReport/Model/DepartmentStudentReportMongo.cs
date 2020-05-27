using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DLL.MongoReport.Model
{
    public class DepartmentStudentReportMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        
    }
}