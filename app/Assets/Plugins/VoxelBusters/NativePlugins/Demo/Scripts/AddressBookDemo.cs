using System.Text;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.NativePlugins;

namespace VoxelBusters.NativePlugins.Demo
{
    public class AddressBookDemo : DemoBase<AddressBookDemoActionButton, AddressBookDemoActionType>
    {
        #region UI callback methods

        protected override void OnButtonPress(AddressBookDemoActionButton selectedButton)
        {
            switch (selectedButton.ActionType)
            {
                case AddressBookDemoActionType.GetAuthStatus:
                    Log("Current authorization status: " + AddressBook.GetAuthorizationStatus());
                    break;

                case AddressBookDemoActionType.ReadContacts:
                    AddressBook.ReadContacts((result) =>
                    {
                        if (result.Error == null)
                        {
                            IAddressBookContact[] contacts = result.Contacts;
                            Log("Read completed successfully.");
                            Log("Total contacts fetched:" + contacts.Length);
                            Log("Printing first 10 contact info");
                            for (int iter = 0; iter < contacts.Length && iter < 10; iter++)
                            {
                                Log(string.Format("[{0}]: {1}", iter, contacts[iter]));
                            }
                        }
                        else
                        {
                            Log("Read completed with error: " + result.Error);
                        }
                    });
                    break;

                case AddressBookDemoActionType.ResourcePage:
                    Application.OpenURL(Internal.Constants.kAddressBookResourcePage);
                    break;

                default:
                    break;
            }
        }

        #endregion
    }
}
