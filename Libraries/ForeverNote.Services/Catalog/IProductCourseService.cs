using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Courses;
using System.Threading.Tasks;

namespace ForeverNote.Services.Catalog
{
    public interface IProductCourseService
    {
        Task<Product> GetProductByCourseId(string courseId);
        Task<Course> GetCourseByProductId(string productId);
        Task UpdateCourseOnProduct(string productId, string courseId);
    }
}
