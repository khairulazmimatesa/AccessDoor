using System;

namespace AccessDoor.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Key { get; set; }

        public string Url { get; set; }

        public string DeviceToken { get; set; }

        public DateTime DeviceTokenExpiredDate { get; set; }

    }
}
