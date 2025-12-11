using System.Collections.Generic;
using EducationPortal.Web.Models;

namespace EducationPortal.Web.Repositories
{
    public interface ICourseRepository
    {
        List<Course> GetAll();
        Course GetById(int id);
        void Add(Course course);
        void Update(Course course);
        void Delete(int id);
    }
}
