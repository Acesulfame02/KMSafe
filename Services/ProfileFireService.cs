using Firebase.Database;
using Firebase.Database.Query;
using Safe.Model;

namespace Safe.Services
{
    internal class ProfileFireService
    {
        private readonly FirebaseClient _firebaseClient;
        private const string ProfilesNode = "UserInfo";

        public ProfileFireService()
        {
            _firebaseClient = new FirebaseClient("https://kmsafe-67573-default-rtdb.firebaseio.com/");
        }

        private string FormatUserId(string userId)
        {
            return userId.Replace(".", ",");
        }

        public async Task<ProfileModel> GetUserProfileAsync(string userId)
        {
            var formattedUserId = FormatUserId(userId);
            var profile = await _firebaseClient
                .Child(ProfilesNode)
                .Child(formattedUserId)
                .OnceSingleAsync<ProfileModel>();

            if (profile == null)
            {
                profile = new ProfileModel
                {
                    Username = "None",
                    CampusAddress = "None",
                    HealthId = "None",
                    PhoneNumber = "None",
                    Email = userId // Save the original email
                };

                await UpdateUserProfileAsync(userId, profile);
            }

            return profile;
        }

        public async Task UpdateUserProfileAsync(string userId, ProfileModel profile)
        {
            var formattedUserId = FormatUserId(userId);

            // Fetch the existing profile to ensure health ID isn't updated
            var existingProfile = await _firebaseClient
                .Child(ProfilesNode)
                .Child(formattedUserId)
                .OnceSingleAsync<ProfileModel>();

            if (existingProfile != null)
            {
                profile.HealthId = existingProfile.HealthId;
            }

            await _firebaseClient
                .Child(ProfilesNode)
                .Child(formattedUserId)
                .PutAsync(profile);
        }
    }
}
