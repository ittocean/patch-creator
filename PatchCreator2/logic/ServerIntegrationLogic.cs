using Renci.SshNet;
using Renci.SshNet.Messages.Authentication;
using System;
using System.IO;

namespace PatchCreator.logic
{
    public enum Status
    {
        Successful,
        Failed_Connect,
        Failed_Transfer,
        Failed_Command,
        Unknown
    }

    public class ServerIntegrationLogic
    {
        private readonly String hostname;
        private readonly String username;
        private readonly String password;

        public ServerIntegrationLogic(string hostname, string username, string password)
        {
            this.hostname = hostname;
            this.username = username;
            this.password = password;
        }

        public Status CopyPatchToServer(string source, string destinationPath)
        {
            Status status = Status.Unknown;
            try
            {
                using (var client = new ScpClient(hostname, username, password))
                {
                    status = Status.Failed_Connect;
                    client.Connect();
                    using (var localFile = File.OpenRead(source))
                    {
                        status = Status.Failed_Transfer;
                        string destinationFile = Path.Combine(destinationPath, Path.GetFileName(source)).Replace("\\", "/");
                        client.Upload(localFile, destinationFile);
                        status = Status.Successful;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            return status;
        }

        public Status ExecuteCommandOnServer(string command)
        {
            Status status = Status.Unknown;
            try
            {
                using (var client = new SshClient(hostname, username, password))
                {
                    status = Status.Failed_Connect;
                    client.Connect();
                    status = Status.Failed_Command;
                    var sshCommand = client.RunCommand(command);
                    sshCommand.Execute();
                    if (sshCommand.ExitStatus == 0)
                    {
                        status = Status.Successful;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            return status;
        }
    }
}
