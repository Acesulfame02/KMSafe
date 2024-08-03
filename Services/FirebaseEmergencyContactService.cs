using Firebase.Database;
using Safe.Model;

namespace Safe.Services
{
    public class FirebaseEmergencyContactService
    {
        private readonly FirebaseClient _firebaseClient;
        private const string HostelsNode = "Hostel";
        private const string ClinicsNode = "Clinic/Nurses";

        public FirebaseEmergencyContactService()
        {
            _firebaseClient = new FirebaseClient("https://kmsafe-67573-default-rtdb.firebaseio.com/");
        }

        public async Task<List<EmergencyContactModel>> GetHostelContactsAsync()
        {
            // Adjust the return type to match the actual Firebase data structure
            var hostels = await _firebaseClient
                .Child(HostelsNode)
                .OnceAsync<Dictionary<string, Dictionary<string, string>>>(); // Assuming nested dictionaries

            var contacts = new List<EmergencyContactModel>();

            // Iterate through top-level key-value pairs (hostels)
            foreach (var hostelKeyValuePair in hostels)
            {
                var hostelContacts = hostelKeyValuePair.Object; // Access inner dictionary of contacts

                // Iterate through contact key-value pairs within the hostel
                foreach (var contactKeyValuePair in hostelContacts)
                {
                    var contact = new EmergencyContactModel
                    {
                        Name = contactKeyValuePair.Value["name"], // Assuming "name" is the key
                        Phone = contactKeyValuePair.Value["phone"], // Assuming "phone" is the key
                        Available = contactKeyValuePair.Value["available"] // Assuming "available" is the key
                    };

                    // Add contact only if available is "yes" (case-insensitive)
                    if (contact.Available.Equals("yes", StringComparison.OrdinalIgnoreCase))
                    {
                        contacts.Add(contact);
                    }
                }
            }

            return contacts;
        }



        public async Task<List<EmergencyContactModel>> GetClinicContactsAsync()
        {
            // Adjust the return type to match the actual Firebase data structure
            var clinics = await _firebaseClient
                .Child(ClinicsNode)
                .OnceAsync<Dictionary<string, string>>(); // Assuming top-level key-value pairs

            var contacts = new List<EmergencyContactModel>();

            // Iterate through the collection of FirebaseObjects
            foreach (var clinicObject in clinics)
            {
                // Access the actual data dictionary within the FirebaseObject
                var clinicData = clinicObject.Object;

                // Extract contact information using key-value access
                var contact = new EmergencyContactModel
                {
                    Name = clinicData["name"], // Assuming "name" is the key
                    Phone = clinicData["phone"], // Assuming "phone" is the key
                    Available = clinicData["available"] // Assuming "available" is the key
                };

                // Add contact only if available is "yes" (case-insensitive)
                if (contact.Available.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    contacts.Add(contact);
                }
            }

            return contacts;
        }


    }
}
