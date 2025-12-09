using System.Collections.Generic;
using EducationPortal.Web.Models;

namespace EducationPortal.Web.Repositories
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        // Kursa özel ekstra işlemler gerekiyorsa buraya eklersin.
        IEnumerable<Course> GetCoursesWithCategory();
    }
}
