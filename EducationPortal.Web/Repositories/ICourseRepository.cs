using EducationPortal.Web.Models;
using System.Collections.Generic;


namespace EducationPortal.Web.Repositories
{
    public interface ICourseRepository
    {
        //Interface, hangi işlemleri desteklediğimizi tanımlar ama nasıl yapılacağını söylemez.
        IEnumerable<Course> GetAll();//Tüm dersleri getir
        Course GetById(int id);//Id'ye göre dersi getir
        void Add(Course course);//Yeni ders ekle
        void Update(Course course);//Dersi Güncelle
        void Delete(int id);//Dersi sil
        void Save();//Değişiklikleri Kaydet
    }
}
