#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using MagnifierMemes.Memes;
using RadLibrary.Configuration;
using RadLibrary.Configuration.Managers;
using RadLibrary.Configuration.Scheme;

#endregion

namespace MagnifierMemes
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Environment.CurrentDirectory = Path.GetDirectoryName(typeof(Program).Assembly.Location) ??
                                               Path.GetDirectoryName(Application.ExecutablePath) ??
                                               Environment.CurrentDirectory;
            }
            catch
            {
            }

            if (!File.Exists("memes.conf"))
                File.Create("memes.conf");

            // permissions fix
            try
            {
                var fSecurity = File.GetAccessControl("memes.conf");
                fSecurity.AddAccessRule(new FileSystemAccessRule(
                    new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                    FileSystemRights.FullControl, AccessControlType.Allow));
                File.SetAccessControl("memes.conf", fSecurity);
            }
            catch
            {
            }
            // /permissions fix

            await Task.Delay(100);

            var config = AppConfiguration.Initialize<FileManager>("memes");

            var scheme = new ConfigurationScheme();

            var memeType = typeof(IMeme);
            var memes = typeof(Program).Assembly.ExportedTypes
                .Where(x => memeType.IsAssignableFrom(x) && !x.IsInterface).ToList();

            foreach (var meme in memes)
            {
                var attr = meme.GetCustomAttribute<MemeAttribute>();

                scheme.AddParameter(attr.Name, false, attr.Description, typeof(string));

                if (attr.AdditionalParameters == null || attr.AdditionalParameters.Length == 0) continue;

                foreach (var parameter in attr.AdditionalParameters)
                    scheme.AddParameter(parameter, false, $"Additional parameter for '{attr.Name}'", typeof(string));
            }

            config.EnsureScheme(scheme);

            var l = new List<Task>();

            foreach (var meme in memes)
            {
                if (!config.GetBool(meme.GetCustomAttribute<MemeAttribute>().Name))
                    continue;

                var createdMeme = (IMeme) Activator.CreateInstance(meme,
                    meme.GetConstructors().First().GetParameters().Length == 0 ? new object[0] : new object[] {config});
                var t = Task.Run(createdMeme.Execute);
                l.Add(t);
            }

            Task.WaitAll(l.ToArray());

            await Task.Delay(-1);
        }
    }
}