using System;
using System.Reflection;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace cross_t_test
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("press ENTER to begin");
            Console.ReadLine();
            string company = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(
                    Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))
                .Company;
            string product = ((AssemblyProductAttribute)Attribute.GetCustomAttribute(
                    Assembly.GetExecutingAssembly(), typeof(AssemblyProductAttribute), false))
                .Product;
            // Console.WriteLine(company+" "+product);
            Logging.WriteLog("старт");
            WriteKey(company, product);
            Console.WriteLine("press ENTER to exit");
            Console.ReadLine();
        }

        private static void WriteKey(string company, string product)
        {
            try
            {
                var key = Registry.LocalMachine.OpenSubKey("Software", true);
                key?.CreateSubKey(company, RegistryKeyPermissionCheck.ReadWriteSubTree);
                key = key?.OpenSubKey(company, true);
                key?.CreateSubKey(product, RegistryKeyPermissionCheck.ReadWriteSubTree);
                key = key?.OpenSubKey(product, true);
                key?.SetValue("URL", "localhost", RegistryValueKind.String);
                Logging.WriteLog("записали ключ в реестр");
                RegAccess(key);
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message);
            }
        }

        private static void RegAccess(RegistryKey key)
        {
            try
            {
                var rs = new RegistrySecurity();
                var currentUser = Environment.UserDomainName + "\\" + Environment.UserName;
                //var users = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
                //NTAccount usersAc = users.Translate(typeof(NTAccount)) as NTAccount;
                //RegistryAccessRule usersDeny = new RegistryAccessRule(
                //    identity: usersAc?.ToString(),
                //    registryRights: RegistryRights.ReadKey,
                //    inheritanceFlags: InheritanceFlags.None,
                //    propagationFlags: PropagationFlags.NoPropagateInherit,
                //    type: AccessControlType.Deny);
                //rs.AddAccessRule(usersDeny);
                rs.AddAccessRule(new RegistryAccessRule(currentUser,
                    RegistryRights.ReadKey,
                    InheritanceFlags.None,
                    PropagationFlags.NoPropagateInherit,
                    AccessControlType.Allow));

                key.SetAccessControl(rs);
                key.Close();
                Logging.WriteLog("Задали права");

            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message);
            }
        }
    }
}
