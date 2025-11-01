using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace EducationPortal.Web.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly EducationContext _context;

        public CourseRepository(EducationContext context)
        {
            _context = context;
        }

        public IEnumerable<Course> GetAll()
        {
            return _context.Courses.ToList();
        }

        public Course GetById(int id)
        {
            return _context.Courses.FirstOrDefault(c => c.CourseId == id);
        }

        public void Add(Course course)
        {
            _context.Courses.Add(course);
        }

        public void Update(Course course)
        {
            _context.Courses.Update(course);
        }

        public void Delete(int id)
        {
            var course = _context.Courses.Find(id);
            if (course != null)
                _context.Courses.Remove(course);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
