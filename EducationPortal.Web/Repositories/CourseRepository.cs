using System.Collections.Generic;
using System.Linq;
using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using Microsoft.EntityFrameworkCore;          

namespace EducationPortal.Web.Repositories    
{
    public class CourseRepository : ICourseRepository
    {
        private readonly EducationContext _context;

        public CourseRepository(EducationContext context)
        {
            _context = context;
        }

        public List<Course> GetAll()
        {
            
            return _context.Courses
                           .Include(c => c.CourseCategory)
                           .ToList();
        }

        public Course GetById(int id)
        {
            return _context.Courses
                           .Include(c => c.CourseCategory)
                           .FirstOrDefault(c => c.CourseId == id);
        }

        public void Add(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        public void Update(Course course)
        {
            _context.Courses.Update(course);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var course = _context.Courses.Find(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
        }
    }
}
