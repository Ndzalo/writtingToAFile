using Assignment8.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Assignment8.Services
{
    public class ProfileService
    {
        private readonly string _filePath;

        public ProfileService()
        {
            _filePath = Path.Combine(FileSystem.AppDataDirectory, "profile.json");
        }

        public async Task SaveProfileAsync(Profile profile)
        {
            var json = JsonSerializer.Serialize(profile);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<Profile> LoadProfileAsync()
        {
            if (!File.Exists(_filePath))
                return new Profile();

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<Profile>(json) ?? new Profile();
        }
         
        public async Task<string> SaveProfilePictureAsync(FileResult photo)
        {
            if (photo == null)
                return string.Empty;

            var newFile = Path.Combine(FileSystem.AppDataDirectory, "profile_picture.jpg");
            using var stream = await photo.OpenReadAsync();
            using var newStream = File.OpenWrite(newFile);
            await stream.CopyToAsync(newStream);

            return newFile;
        }
    }
}
