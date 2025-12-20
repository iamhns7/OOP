using System;
using System.Collections.Generic;
using CompanyTaskProjectManagement.Entities;
using CompanyTaskProjectManagement.Repositories;

namespace CompanyTaskProjectManagement.Services
{
    /// <summary>
    /// Kullanıcı Service - İş mantığı katmanı
    /// Single Responsibility Principle: Sadece kullanıcı işlemleriyle ilgilenir
    /// </summary>
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        // Dependency Injection (Constructor Injection)
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public void AddUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            ValidateUser(user);

            // Kullanıcı adı kontrolü
            var existingUser = _userRepository.GetByUsername(user.KullaniciAdi);
            if (existingUser != null)
                throw new InvalidOperationException("Bu kullanıcı adı zaten kullanılıyor!");

            _userRepository.Add(user);
        }

        public void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            ValidateUser(user);

            var existingUser = _userRepository.GetById(user.Id);
            if (existingUser == null)
                throw new InvalidOperationException("Kullanıcı bulunamadı!");

            _userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                throw new InvalidOperationException("Kullanıcı bulunamadı!");

            _userRepository.Delete(id);
        }

        public User Authenticate(string kullaniciAdi, string sifre)
        {
            if (string.IsNullOrWhiteSpace(kullaniciAdi))
                throw new ArgumentException("Kullanıcı adı boş olamaz!", nameof(kullaniciAdi));

            if (string.IsNullOrWhiteSpace(sifre))
                throw new ArgumentException("Şifre boş olamaz!", nameof(sifre));

            return _userRepository.Authenticate(kullaniciAdi, sifre);
        }

        public User Login(string kullaniciAdi, string sifre)
        {
            return Authenticate(kullaniciAdi, sifre);
        }

        private void ValidateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.AdSoyad))
                throw new ArgumentException("Ad Soyad boş olamaz!");

            if (string.IsNullOrWhiteSpace(user.KullaniciAdi))
                throw new ArgumentException("Kullanıcı adı boş olamaz!");

            if (string.IsNullOrWhiteSpace(user.Sifre))
                throw new ArgumentException("Şifre boş olamaz!");

            if (user.Sifre.Length < 3)
                throw new ArgumentException("Şifre en az 3 karakter olmalıdır!");
        }
    }
}
