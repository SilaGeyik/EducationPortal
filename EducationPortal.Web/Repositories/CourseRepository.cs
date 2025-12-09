using System.Collections.Generic;
using System.Linq;
using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Web.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(EducationContext context) : base(context)
        {
        }

        public IEnumerable<Course> GetCoursesWithCategory()
        {
            // Course modelinde Category navigation property varsa:
            return _dbSet
                .Include(c => c.CourseCategory)
                .ToList();
        }
    }
}
