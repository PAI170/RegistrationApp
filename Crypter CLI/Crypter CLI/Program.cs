using System.CommandLine;
using CrypterCLI;


RootCommand rootCommand = new RootCommand("Crypter CLI - Encrypt, decrypt, and hash data");


rootCommand.AddCommand(EncryptCommand.CreateCommand());
rootCommand.AddCommand(DecryptCommand.CreateCommand());
rootCommand.AddCommand(HashCommand.CreateCommand());


rootCommand.Invoke(args);

// For testing purposes, you can uncomment the line below
// rootCommand.Invoke(new string[] { "encrypt", "--input", "Hello World", "--text", "--password", "secret123" });