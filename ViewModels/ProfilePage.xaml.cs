using Assignment8.Models;
using Microsoft.Maui;
using Assignment8.Services;

namespace Assignment8.ViewModels;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileService _profileService;
    private Profile _currentProfile;

    public ProfilePage()
    {
        InitializeComponent();
        _profileService = new ProfileService();
        LoadProfile();
    }

    private async void LoadProfile()
    {
        _currentProfile = await _profileService.LoadProfileAsync();

        NameEntry.Text = _currentProfile.Name;
        SurnameEntry.Text = _currentProfile.Surname;
        EmailEntry.Text = _currentProfile.Email;
        BioEditor.Text = _currentProfile.Bio;

        if (!string.IsNullOrEmpty(_currentProfile.ProfilePicturePath))
        {
            ProfileImage.Source = _currentProfile.ProfilePicturePath;
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(NameEntry.Text) ||
            string.IsNullOrEmpty(SurnameEntry.Text) ||
            string.IsNullOrEmpty(EmailEntry.Text) ||
            string.IsNullOrEmpty(BioEditor.Text))
        {
            await DisplayAlert("Error", "All fields must be filled out.", "OK");
            return;
        }

        _currentProfile.Name = NameEntry.Text;
        _currentProfile.Surname = SurnameEntry.Text;
        _currentProfile.Email = EmailEntry.Text;
        _currentProfile.Bio = BioEditor.Text;

        await _profileService.SaveProfileAsync(_currentProfile);
        await DisplayAlert("Success", "Profile saved successfully!", "OK");

        // Clear the fields after saving, except for the photo
        NameEntry.Text = string.Empty;
        SurnameEntry.Text = string.Empty;
        EmailEntry.Text = string.Empty;
        BioEditor.Text = string.Empty;
        // Leave the photo as is
    }

    private async void OnProfileImageTapped(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.PickPhotoAsync();
            if (photo != null)
            {
                var photoPath = await _profileService.SaveProfilePictureAsync(photo);
                _currentProfile.ProfilePicturePath = photoPath;
                ProfileImage.Source = photoPath;
                await _profileService.SaveProfileAsync(_currentProfile);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to pick photo: " + ex.Message, "OK");
        }
    }
}
