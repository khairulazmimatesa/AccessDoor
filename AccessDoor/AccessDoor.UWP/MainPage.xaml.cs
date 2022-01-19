using System;
using Windows.System.Profile;
using Xamarin.Essentials;

namespace AccessDoor.UWP
{
    public sealed partial class MainPage
    {
        public MainPage() {
            this.InitializeComponent();

            //Generate Token For Windows
            var id = Preferences.Get("my_id", string.Empty);

            if (string.IsNullOrWhiteSpace(id)) {
                var token = HardwareIdentification.GetPackageSpecificToken(null);
                var hardwareId = token.Id;
                var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

                byte[] bytes32 = new byte[hardwareId.Length];
                dataReader.ReadBytes(bytes32);

                var hex = BitConverter.ToString(bytes32).Replace("-", "");

                var numberChars = hex.Length;
                var bytes = new byte[numberChars / 4];
                for (var i = 0; i < numberChars; i += 4)
                    bytes[i / 8] = Convert.ToByte(hex.Substring(i, 2), 16);

                id = BitConverter.ToString(bytes).Replace("-", "").Replace("00", "").ToLower();
            }

            Preferences.Set("my_id", id);

            LoadApplication(new AccessDoor.App(id));
        }
    }
}
